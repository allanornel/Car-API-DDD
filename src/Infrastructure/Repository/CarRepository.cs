using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public CarRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            string query = @"SELECT * FROM Car c
                     INNER JOIN Photo p ON c.PhotoId = p.Id
                     WHERE c.Id = @Id";
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();

            var car = await dbConnection.QueryAsync<Car, Photo, Car>(
                query,
                (car, photo) =>
                {
                    car.Photo = photo;
                    return car;
                },
                param: new { Id = id }
            );

            return car.FirstOrDefault();
        }

        public async Task<Tuple<IEnumerable<Car>, int, int>> GetAllAsync(string? searchQuery, int page)
        {
            const int PAGE_SIZE = 10;
            string whereClause = "WHERE Status = 1";
            string queryCount = @"SELECT COUNT(*) FROM Car c
                          INNER JOIN Photo p ON c.PhotoId = p.Id ";

            string query = @"SELECT * FROM Car c
                     INNER JOIN Photo p ON c.PhotoId = p.Id ";

            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                whereClause += " AND c.Name LIKE @SearchQuery";
                parameters.Add("@SearchQuery", "%" + searchQuery + "%");
            }
            query += whereClause;
            queryCount += whereClause;

            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();

            int totalCars = await dbConnection.ExecuteScalarAsync<int>(queryCount, parameters);
            int totalPages = (int)Math.Ceiling((double)totalCars / PAGE_SIZE);

            query += " ORDER BY c.id desc OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("@Offset", (page - 1) * PAGE_SIZE);
            parameters.Add("@PageSize", PAGE_SIZE);

            var cars = await dbConnection.QueryAsync<Car, Photo, Car>(
                query,
                (car, photo) =>
                {
                    car.Photo = photo;
                    return car;
                },
                param: parameters
            );

            return new Tuple<IEnumerable<Car>, int, int>(cars, totalCars, totalPages);
        }

        public async Task<Car> AddAsync(string name, string base64)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        string photoQuery = @"INSERT INTO Photo(Base64) VALUES (@Base64);
                                      SELECT SCOPE_IDENTITY();";
                        int photoId = await dbConnection.ExecuteScalarAsync<int>(photoQuery, new { base64 }, transaction);

                        string carQuery = @"INSERT INTO Car (PhotoId, Name, Status) VALUES (@PhotoId, @Name, 1); SELECT SCOPE_IDENTITY();";
                        int createdCarId = await dbConnection.ExecuteScalarAsync<int>(carQuery, new { PhotoId = photoId, Name = name }, transaction);

                        transaction.Commit();

                        return await GetByIdAsync(createdCarId);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task UpdateAsync(Car car)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        string photoQuery = @"UPDATE Photo SET Base64 = @Base64 WHERE Id = @PhotoId";
                        await dbConnection.ExecuteAsync(photoQuery, new { Base64 = car.Photo.Base64, PhotoId = car.Photo.Id }, transaction);
                        string carQuery = @"UPDATE Car SET Name = @Name WHERE Id = @Id";
                        await dbConnection.ExecuteAsync(carQuery, new { Name = car.Name, Id = car.Id }, transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            string query = "UPDATE Car SET Status = 0 WHERE Id = @Id";
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            int rowAffected = await dbConnection.ExecuteAsync(query, new { Id = id });
            return rowAffected;
        }
    }
}

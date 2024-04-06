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
            string query = @"SELECT c.*, p.Id as PhotoId, p.Base64 as PhotoBase64 FROM Car c
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
                splitOn: "PhotoId",
                param: new { Id = id }
            );

            return car.FirstOrDefault();
        }

        public async Task<IEnumerable<Car>> GetAllAsync(string searchQuery, int page)
        {
            const int PAGE_SIZE = 10;
            string query = @"SELECT c.*, p.Id as PhotoId, p.Base64 as PhotoBase64 FROM Car c
                     INNER JOIN Photo p ON c.PhotoId = p.Id
                     WHERE Status = 1";
            DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += " AND c.Name LIKE @SearchQuery";
                parameters.Add("@SearchQuery", "%" + searchQuery + "%");
            }

            query += " ORDER BY c.Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("@Offset", page * PAGE_SIZE);
            parameters.Add("@PageSize", PAGE_SIZE);

            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            var cars = await dbConnection.QueryAsync<Car, Photo, Car>(
                query,
                (car, photo) =>
                {
                    car.Photo = photo;
                    return car;
                },
                splitOn: "PhotoId",
                param: parameters
            );

            return cars;
        }

        public async Task AddAsync(Car car)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        string photoQuery = @"INSERT INTO Photo(Base64) VALUES (@Base64);
                                      SELECT SCOPE_IDENTITY();";
                        int photoId = await dbConnection.ExecuteScalarAsync<int>(photoQuery, new { car.Photo.Base64 }, transaction);

                        string carQuery = @"INSERT INTO Car (PhotoId, Name, Status) VALUES (@PhotoId, @Name, @Status);";
                        await dbConnection.ExecuteAsync(carQuery, new { PhotoId = photoId, car.Name, car.Status }, transaction);

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

        public async Task UpdateAsync(Car car)
        {
            string query = "UPDATE Car SET PhotoId = @PhotoId, Name = @Name, Status = @Status WHERE Id = @Id";
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            await dbConnection.ExecuteAsync(query, car);
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

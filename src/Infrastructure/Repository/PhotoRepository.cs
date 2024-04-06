using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository
{
    public class PhotoRepository : IPhotoRepository
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PhotoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Photo?> GetByIdAsync(int id)
        {
            string query = "SELECT * FROM Photo WHERE Id = @Id";
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            return await dbConnection.QueryFirstOrDefaultAsync<Photo>(query, new { Id = id });
        }

        public async Task<Photo?> AddAsync(string base64)
        {
            string query = "INSERT INTO Photo (Base64) VALUES (@Base64); SELECT SCOPE_IDENTITY();";
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            int photoId = await dbConnection.QueryFirstOrDefaultAsync<int>(query, new { Base64 = base64 });
            if (photoId == 0)
            {
                return null;
            }
            return await GetByIdAsync(photoId);
        }
    }
}

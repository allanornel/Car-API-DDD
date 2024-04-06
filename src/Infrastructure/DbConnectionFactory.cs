using Domain.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

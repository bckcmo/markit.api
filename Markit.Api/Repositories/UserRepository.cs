using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Markit.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;
        private IDbConnection connection => new MySqlConnection(connectionString);

        public UserRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("MySql");
        }

        public async Task<UserEntity> GetById(int id)
        {
            using var conn = connection;
            var query = "SELECT * from users WHERE id = @id";
            conn.Open();
            var result = await conn.QuerySingleAsync<UserEntity>(query, new {Id = id});
            return result;
        }

        public async Task<UserEntity> CreateUser(UserRegistration user)
        {
            using var conn = connection;
            var insertQuery =
                @"INSERT into users (FirstName, LastName, Email, Password) Values (@FirstName, @LastName, @Email, @Password);
                    SELECT * from users WHERE id = LAST_INSERT_ID()";
            
            conn.Open();
            
            var result = await conn.QueryAsync<UserEntity>(insertQuery, user);
            return result.Single();
        }
    }
}
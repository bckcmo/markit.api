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
            var query = "SELECT Id, FirstName, LastName, Email, Reputation FROM users WHERE id = @id";
            conn.Open();
            var result = await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new {Id = id});
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

        public async Task<UserEntity> GetByEmail(string email)
        {
            using var conn = connection;
            var query = "SELECT Id, FirstName, LastName, Email, Reputation FROM users WHERE Email = @email";
            conn.Open();
            var result = await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new {Email = email});
            return result;
        }
        
        public async Task<UserEntity> Update(User user)
        {
            using var conn = connection;
            var query = @"UPDATE users SET Id = @Id, FirstName = @FirstName, LastName = @LastName, 
            Email = @Email, Reputation = @Reputation WHERE Id = @Id";
            conn.Open();
            var result = await conn.QuerySingleOrDefaultAsync<UserEntity>(query, user);
            return result;
        }
        
        public async Task<UserEntity> Delete(int id)
        {
            using var conn = connection;
            var query = @"DELETE FROM users WHERE Id = @Id";
            conn.Open();
            var result = await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new {Id = id});
            return result;
        }
    }
}
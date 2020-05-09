using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using MySql.Data.MySqlClient;

namespace Markit.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        
        private IDbConnection connection => new MySqlConnection(_connectionString);

        public UserRepository(IDatabaseUtil databaseUtil)
        {
            _connectionString = databaseUtil.GetConnectionString();
        }

        public async Task<UserEntity> GetById(int id)
        {
            using var conn = connection;
            var query = "SELECT * FROM users WHERE id = @id";
            conn.Open();
            var result = await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new {Id = id});
            return result;
        }

        public async Task<List<UserEntity>> GetByIds(List<int> ids)
        {
            using var conn = connection;
            var query = "SELECT * FROM users WHERE Id IN @ids";
            conn.Open();
            var result = await conn.QueryAsync<UserEntity>(query, new { ids });
            return result.ToList();
        }

        public async Task<UserEntity> CreateUser(UserRegistration user)
        {
            using var conn = connection;
            var insertQuery =
                @"INSERT into users (FirstName, LastName, UserName, Password) Values (@FirstName, @LastName, @UserName, @Password);
                    SELECT * from users WHERE id = LAST_INSERT_ID()";
            
            conn.Open();
            
            var result = await conn.QueryAsync<UserEntity>(insertQuery, user);
            return result.Single();
        }

        public async Task<UserEntity> GetByUserName(string userName)
        {
            using var conn = connection;
            var query = "SELECT * FROM users WHERE UserName = @userName";
            conn.Open();
            var result = await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new {UserName = userName});
            return result;
        }
        
        public async Task<UserEntity> Update(User user)
        {
            using var conn = connection;
            var query = @"UPDATE users SET FirstName = @FirstName, LastName = @LastName, 
                        UserName = @UserName, Reputation = @Reputation WHERE Id = @Id;
                        SELECT * FROM users WHERE Id = @Id";
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
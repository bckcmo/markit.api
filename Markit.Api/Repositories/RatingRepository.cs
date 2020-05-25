using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using MySql.Data.MySqlClient;

namespace Markit.Api.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly string _connectionString;
        private IDbConnection connection => new MySqlConnection(_connectionString);
        
        public RatingRepository(IDatabaseUtil databaseUtil)
        { 
            _connectionString = databaseUtil.GetConnectionString();
        }

        public async Task<RatingEntity> CreateAsync(Rating rating)
        {
            using var conn = connection;
            var query = @"INSERT INTO ratings (UserId, StoreId, Comment, Points) 
                        VALUES (@UserId, @StoreId, @Comment, @Points);
                        UPDATE stores SET AverageRating = (SELECT ROUND(AVG(Points), 1) 
                        FROM ratings WHERE StoreId = @StoreId) WHERE Id = @StoreId;
                        SELECT * FROM ratings WHERE id = LAST_INSERT_ID();";

            conn.Open();

            var result = await conn.QuerySingleAsync<RatingEntity>(query, new
            {
                UserId = rating.UserId,
                StoreId = rating.Store.Id,
                Comment = rating.Comment,
                Points = rating.Points
            });

            return result;
        }

        public async Task<List<RatingEntity>> GetRatingsForStoresAsync(IEnumerable<int> storeIds)
        {
            using var conn = connection;
            var query = @"SELECT * FROM ratings WHERE storeId IN @storeIds ORDER BY CreatedAt DESC LIMIT 20";
            
            conn.Open();

            var results = await conn.QueryAsync<RatingEntity>(query, new { storeIds });

            return results.ToList();
        }

        public async Task<List<RatingEntity>> GetRecentRatingsAsync(int userId)
        {
            using var conn = connection;
            var query = @"SELECT * FROM ratings WHERE UserId = @userId ORDER BY CreatedAt DESC LIMIT 20";
            
            conn.Open();

            var results = await conn.QueryAsync<RatingEntity>(query, new { userId });

            return results.ToList();
        }

        public async Task<int> GetRatingsCountByUserIdAsync(int userId)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT COUNT(Id) from ratings where userId = @userId ";
            var results = await conn.QuerySingleAsync<int>(query, new { userId });
            
            return results;
        }
        
        public async Task<int> GetRatingsCountByStoreIdAsync(int storeId)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT COUNT(Id) from ratings where storeId = @storeId ";
            var results = await conn.QuerySingleAsync<int>(query, new { storeId });
            
            return results;
        }

        public async Task<IList<RatingEntity>> GetRatingsForStoreAsync(int storeId)
        {
            using var conn = connection;
            var query = @"SELECT * FROM ratings WHERE StoreId = @storeId ORDER BY CreatedAt DESC LIMIT 20";
            
            conn.Open();

            var results = await conn.QueryAsync<RatingEntity>(query, new { storeId });

            return results.ToList();
        }
    }
}
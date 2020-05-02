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
                        SELECT * FROM ratings WHERE id = LAST_INSERT_ID()";

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
    }
}
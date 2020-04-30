using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Entities;
using MySql.Data.MySqlClient;

namespace Markit.Api.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly string _connectionString;
        
        private IDbConnection connection => new MySqlConnection(_connectionString);
        
        public TagRepository(IDatabaseUtil databaseUtil)
        {
            _connectionString = databaseUtil.GetConnectionString();
        }

        public async Task<IEnumerable<TagEntity>> QueryTags(string tag, int limit)
        {
            using var conn = connection;
            
            var query = @"SELECT Id, Name, CreatedAt FROM tags where Name LIKE @term LIMIT @limit";
            
            conn.Open();
            
            var result = await conn.QueryAsync<TagEntity>(query, new
            {
                term = $"{tag}%", 
                limit
            });
            
            return result;
        }
        
        private async Task<TagEntity> CreateTag(string name)
        {
            using var conn = connection;
            
            var insertQuery =
                @"INSERT into tags (Name) Values (@name);
                    SELECT * from tags WHERE id = LAST_INSERT_ID()";

            conn.Open();
            
            var result = await conn.QueryAsync<TagEntity>(insertQuery, new {Name = name});
            return result.Single();
        }
        
        private async Task<TagEntity> GetTagByName(string name)
        {
            using var conn = connection;
            
            var query = @"SELECT Id, Name, CreatedAt FROM tags where Name = @name";
            
            conn.Open();
            
            var result = await conn.QueryAsync<TagEntity>(query, new {Name = name});
            return result.Single();
        }
    }
}
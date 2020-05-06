using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Comparers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
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

        public async Task<IEnumerable<TagEntity>> QueryTags(string name, string upc, int limit)
        {
            using var conn = connection;

            conn.Open();

            IEnumerable<TagEntity> nameTags = new List<TagEntity>();
            IEnumerable<TagEntity> upcTags = new List<TagEntity>();
            
            if(name != null)
            {
                var nameQuery = @"SELECT Id, Name, CreatedAt FROM tags where Name LIKE @term LIMIT @limit";
                nameTags = await conn.QueryAsync<TagEntity>(nameQuery, new
                {
                    term = $"{name}%", 
                    limit
                });
            }

            if (upc != null)
            {
                var upcQuery = @"SELECT tags.Id, tags.Name, tags.CreatedAt FROM tags 
                JOIN itemTags ON itemTags.TagId = tags.Id
                JOIN items ON items.Id = itemTags.ItemId
                where items.Upc LIKE @term LIMIT @limit";
                upcTags = await conn.QueryAsync<TagEntity>(upcQuery, new
                {
                    term = $"{upc}%", 
                    limit
                });
            }
            
            return nameTags.Union(upcTags).Distinct(new TagEntityCompare());
        }

        public async Task<IEnumerable<ListTagEntity>> GetListTags(int listId)
        {
            using var conn = connection;
            
            var query = @"SELECT * FROM listtags where ListId = @listId";
            
            conn.Open();
            
            var result = await conn.QueryAsync<ListTagEntity>(query, new { listId });
            return result;
        }
        
        public async Task<TagEntity> GetTagById(int id)
        {
            using var conn = connection;
            
            var query = @"SELECT Id, Name, CreatedAt FROM tags where Id = @id";
            
            conn.Open();
            
            var result = await conn.QueryAsync<TagEntity>(query, new { id });
            return result.Single();
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
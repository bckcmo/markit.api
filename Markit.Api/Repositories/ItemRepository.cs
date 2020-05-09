using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using MySql.Data.MySqlClient;

namespace Markit.Api.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _connectionString;
        
        private IDbConnection connection => new MySqlConnection(_connectionString);

        public ItemRepository(IDatabaseUtil databaseUtil)
        {
            _connectionString = databaseUtil.GetConnectionString();
        }
        
        public async Task<ItemEntity> GetItemById(int id)
        {
            using var conn = connection;
            var query =
                @"SELECT Id, Upc, CreatedAt FROM items WHERE Id = @id";
            
            conn.Open();
            
            var result = await conn.QuerySingleAsync<ItemEntity>(query, new { id });
            return result;
        }
        
        public async Task<StoreItemEntity> GetStoreItemById(int id)
        {
            using var conn = connection;
            var query =
                @"SELECT * FROM storeitems WHERE Id = @id";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreItemEntity>(query, new { id });
            return result.Single();
        }
        
        public async Task<StoreItemEntity> GetStoreItemByItemId(int id)
        {
            using var conn = connection;
            var query =
                @"SELECT * FROM storeitems WHERE ItemId = @id";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreItemEntity>(query, new { id });
            return result.Single();
        }

        public async Task<ItemEntity> CreateItem(Item item)
        {
            using var conn = connection;
            var insertQuery =
                @"INSERT into items (Upc) Values (@Upc);
                    SELECT * from items WHERE id = LAST_INSERT_ID()";
            
            conn.Open();
            
            var result = await conn.QueryAsync<ItemEntity>(insertQuery, item);
            return result.Single();
        }

        public async Task DeleteItemById(int id)
        {
            using var conn = connection;
            var query = @"DELETE FROM items WHERE Id = @id;";
            conn.Open();
            var result = await conn.ExecuteAsync(query, new { id });
        }

        public async Task<StoreItemEntity> CreateStoreItem(PostStoreItem item)
        {
            using var conn = connection;
            var updateQuery = @"BEGIN;
                INSERT IGNORE INTO items (Upc) VALUES (@Upc); 
                INSERT IGNORE INTO storeItems (ItemId, StoreId) VALUES ((SELECT Id FROM items WHERE Upc = @Upc), @StoreId); 
                INSERT INTO userPrices (StoreItemId, UserId, Price, IsSalePrice) VALUES (
                (SELECT Id FROM storeItems WHERE StoreId = @StoreId and ItemId = 
                (SELECT Id FROM items WHERE Upc = @Upc)), @UserId, @Price, @IsSalePrice);
                COMMIT; 
                SELECT * FROM storeItems WHERE ItemId = (SELECT Id from items WHERE Upc = @Upc);";
            
            var result = await conn.QuerySingleAsync<StoreItemEntity>(updateQuery, item);
            await Task.WhenAll(item.Tags.Select( t => AddTags(t, item.Upc)));
            return result;
        }

        public async Task<List<UserPriceEntity>> GetUserPricesByItemId(int storeItemId)
        {
            using var conn = connection;
            var query = @"SELECT * from userPrices where StoreItemId = @storeItemId";

            conn.Open();

            var result = await conn.QueryAsync<UserPriceEntity>(query, new
            {
                StoreItemId = storeItemId
            });

            return result.ToList();
        }

        public async Task<List<UserPriceEntity>> GetUserPricesByStoreItemIds(List<int> storeItemIds)
        {
            using var conn = connection;
            
            conn.Open();
            
            var query = @"SELECT * FROM userprices WHERE StoreItemId IN @storeItemIds ORDER BY CreatedAt DESC LIMIT 20";
            
            var results = await conn.QueryAsync<UserPriceEntity>(query, new { storeItemIds });

            return results.ToList();
        }

        private async Task<TagEntity> AddTags(string tag, string upc)
        {
            using var conn = new MySqlConnection(_connectionString);
            var query = @"INSERT IGNORE INTO tags (Name) VALUES (@Tag);
                            INSERT IGNORE INTO itemTags (TagId, ItemId) VALUES (LAST_INSERT_ID(), 
                            (SELECT Id from items WHERE Upc = @Upc))";
    
            var result = new TagEntity();
            try
            {
                result = await conn.QuerySingleAsync<TagEntity>(query, new {Tag = tag, Upc = upc});
            }
            catch
            {
                // noop
            }

            return result;
        }
    }
}
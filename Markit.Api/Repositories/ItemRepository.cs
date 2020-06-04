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
            var query = @"SELECT * FROM storeitems WHERE Id = @id";
            
            conn.Open();
            
            var result = await conn.QuerySingleAsync<StoreItemEntity>(query, new { id });
            return result;
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
                SELECT * FROM storeItems WHERE ItemId = (SELECT Id from items WHERE Upc = @Upc) AND storeId = @storeId;";
            
            var result = await conn.QuerySingleAsync<StoreItemEntity>(updateQuery, item);
            await Task.WhenAll(item.Tags.Select( t => AddTag(t, item.Upc)));
            return result;
        }

        public async Task<List<UserPriceEntity>> GetUserPricesByStoreItemIds(List<int> storeItemIds)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT * FROM userprices WHERE StoreItemId IN @storeItemIds ORDER BY CreatedAt DESC LIMIT 20";
            var results = await conn.QueryAsync<UserPriceEntity>(query, new { storeItemIds });

            return results.ToList();
        }
        
        public async Task<List<UserPriceEntity>> GetUserPricesByUserId(int userId)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT * FROM userprices WHERE UserId = @userId ORDER BY CreatedAt DESC LIMIT 20";
            var results = await conn.QueryAsync<UserPriceEntity>(query, new { userId });
            
            return results.ToList();
        }
        
        public async Task<List<UserPriceEntity>> GetUserPricesByStoreId(int storeId)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT * FROM userprices 
                        JOIN storeItems on storeItems.Id = userPrices.StoreItemId
                        where storeItems.storeId = @storeId
                        ORDER BY CreatedAt DESC LIMIT 20";
            var results = await conn.QueryAsync<UserPriceEntity>(query, new { storeId });
            
            return results.ToList();
        }

        public async Task<UserPriceEntity> GetMostRecentUserPriceByTagName(string tagName, int storeId)
        {
            using var conn = connection;
            conn.Open();

            var query = @"Select * FROM userPrices 
                          JOIN storeItems ON storeItems.Id = userPrices.StoreItemId 
                          WHERE storeItems.StoreId = @storeId 
                          AND itemId IN (SELECT itemId FROM itemtags WHERE tagId = (SELECT id FROM tags WHERE NAME = @tagName)) 
                          ORDER BY userprices.CreatedAt DESC LIMIT 1";
            
            var result = await conn.QueryAsync<UserPriceEntity>(query, new { tagName, storeId });

            return result.FirstOrDefault();
        }
        
        public async Task<int> GetUserPriceCountByUserId(int userId)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT COUNT(Id) from userPrices where userId = @userId ";
            var results = await conn.QuerySingleAsync<int>(query, new { userId });
            
            return results;
        }
        
        public async Task<int> GetUserPriceCountByStoreId(int storeId)
        {
            using var conn = connection;
            conn.Open();
            
            var query = @"SELECT COUNT(userPrices.Id) from userPrices
                        JOIN storeItems on storeItems.Id = userPrices.StoreItemId
                        where storeItems.storeId = @storeId ";
            var results = await conn.QuerySingleAsync<int>(query, new { storeId });
            
            return results;
        }

        private async Task<TagEntity> AddTag(string tag, string upc)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var query = @"INSERT IGNORE INTO tags (Name) VALUES (@tag);
                          INSERT IGNORE INTO itemTags (TagId, ItemId) VALUES (
                            (SELECT Id FROM tags WHERE Name = @tag), 
                            (SELECT Id FROM items WHERE Upc = @upc));
                            SELECT * FROM Tags WHERE Name = @tag";
    
            var result = new TagEntity();
            try
            {
                result = await conn.QuerySingleAsync<TagEntity>(query, new {tag, upc});
            }
            catch
            {
                // noop
            }

            return result;
        }
    }
}
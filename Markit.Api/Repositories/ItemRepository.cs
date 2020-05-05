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
        
        public async Task<ItemEntity> GetItemByUpc(string upc)
        {
            using var conn = connection;
            var query =
                @"SELECT Id, Upc, CreatedAt FROM items WHERE Upc = @upc";
            
            conn.Open();
            
            var result = await conn.QuerySingleAsync<ItemEntity>(query, new {Upc = upc});
            return result;
        }
        
        public async Task<StoreItemEntity> GetStoreItemById(int id)
        {
            using var conn = connection;
            var query =
                @"SELECT Id, Upc, CreatedAt FROM items WHERE Id = @id";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreItemEntity>(query, new {Id = id});
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

        public async Task<StoreItemEntity> CreateStoreItem(PostStoreItem item)
        {
            using var conn = connection;
            var updateQuery = @"BEGIN;
                INSERT IGNORE INTO items (Upc) VALUES (@Upc); 
                INSERT IGNORE INTO storeItems (ItemId, StoreId) VALUES ((SELECT Id FROM items WHERE Upc = @Upc), @StoreId); 
                INSERT INTO userPrices (StoreItemId, UserId, Price, IsSalePrice) VALUES (
                (SELECT Id FROM storeItems WHERE StoreId = @StoreId and ItemId = 
                (SELECT Id FROM items WHERE Upc = @Upc)), @UserId, @Price, @IsSalePrice); 
                INSERT INTO tags (Name) VALUES (@Tag); 
                INSERT INTO itemTags (TagId, ItemId) VALUES (LAST_INSERT_ID(), (SELECT Id FROM items WHERE Upc = @Upc)); 
                COMMIT;
                SELECT * FROM storeItems WHERE ItemId = (SELECT Id from items WHERE Upc = @Upc);";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreItemEntity>(updateQuery, item);
            
            return result.Single();
        }

        public async Task<UserPriceEntity> GetUserPriceByItemId(int storeItemId)
        {
            using var conn = connection;
            var query = @"SELECT * from userPrices where StoreItemId = @storeItemId";

            conn.Open();

            var result = await conn.QueryAsync<UserPriceEntity>(query, new
            {
                StoreItemId = storeItemId
            });

            return result.Single();
        }
    }
}
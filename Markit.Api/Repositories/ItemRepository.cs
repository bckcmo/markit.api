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
        private IStoreRepository _storeRepository;

        public ItemRepository(IDatabaseUtil databaseUtil, IStoreRepository storeRepository)
        {
            _connectionString = databaseUtil.GetConnectionString();
            _storeRepository = storeRepository;
        }
        
        public async Task<ItemEntity> GetItemByUpc(string upc)
        {
            using var conn = connection;
            var query =
                @"SELECT Id, Upc, CreatedAt FROM items WHERE Upc = @upc";
            
            conn.Open();
            
            var result = await conn.QueryAsync<ItemEntity>(query, new {Upc = upc});
            return result.Single();
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

            var query = @"BEGIN; INSERT INTO items (Upc) VALUES (@Upc); 
                INSERT INTO storeItems (ItemId, StoreId) VALUES (LAST_INSERT_ID(), @StoreId); 
                INSERT INTO userPrices (StoreItemId, UserId, Price, IsSalePrice) VALUES (LAST_INSERT_ID(), @UserId, @Price, @IsSalePrice); 
                INSERT INTO tags (Name) VALUES (@Tag); 
                INSERT INTO itemTags (TagId, ItemId) VALUES (LAST_INSERT_ID(), (SELECT Id FROM items WHERE Upc = @Upc)); 
                COMMIT;
                SELECT * FROM storeItems WHERE Id = (SELECT Id from items WHERE Upc = @Upc);";
            
            conn.Open();
            
            var result = await conn.QueryAsync<StoreItemEntity>(query, item);
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

        // TODO move to tag repo
        private async Task<TagEntity> CreateTag(string name, IDbConnection conn)
        {
            var insertQuery =
                @"INSERT into tags (Name) Values (@name);
                    SELECT * from tags WHERE id = LAST_INSERT_ID()";

            var result = await conn.QueryAsync<TagEntity>(insertQuery, new {Name = name});
            return result.Single();
        }

        // TODO move to tag repo
        private async Task<TagEntity> GetTagByName(string name, IDbConnection conn)
        {
            var query = @"SELECT Id, Name, CreatedAt FROM tags where Name = @name";
            
            var result = await conn.QueryAsync<TagEntity>(query, new {Name = name});
            return result.Single();
        }

        private async Task<ItemTagEntity> InsertItemTag(int itemId, int tagId, IDbConnection conn)
        {
            var query = @"INSERT INTO itemTags (ItemId, TagId) VALUES (@itemId, @tagId)";

            var result = await conn.QueryAsync<ItemTagEntity>(query, new
            {
                ItemId = itemId,
                TagId = tagId
            });
            return result.Single();
        }
    }
}
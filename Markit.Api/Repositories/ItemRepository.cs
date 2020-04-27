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

        public Task<ItemEntity> GetItemById(int id)
        {
            throw new NotImplementedException();
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

        public async Task<StoreItemEntity> CreateStoreItem(StoreItem item)
        {
            using var conn = connection;
            
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                ItemEntity existingItem;

                try
                {
                    existingItem = await CreateItem(new Item {Upc = item.Upc});
                }
                catch
                {
                    existingItem = await GetItemByUpc(item.Upc);
                }

                List<TagEntity> tags = new List<TagEntity>();
                
                // TODO add a CreateTags method to tag repo and move iteration there
                foreach (var t in item.Tags)
                {
                    try
                    {
                        var tag = await CreateTag(t, conn);
                        tags.Add(tag);
                    }
                    catch
                    {
                        var tag = await GetTagByName(t, conn);
                        tags.Add(tag);
                    }
                }
                    

                foreach (var t in tags)
                {
                    await InsertItemTag(existingItem.Id, t.Id, conn);
                }

                var store = await _storeRepository.GetStoreById(item.StoreId);
                
                //create userprice, TODO write function
                var userPrice = await CreatUserPrice(item);
                
                //enter storeitem, TODO write function
                var storeItemEntity = await InsertStoreItem(item);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw new Exception("Failed to save StoreItem.");
            }
            
            return storeItemEntity;
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
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
    public class ListRepository : IListRepository
    {
        private readonly string _connectionString;
        private IDbConnection connection => new MySqlConnection(_connectionString);
        
        public ListRepository(IDatabaseUtil databaseUtil)
        { 
            _connectionString = databaseUtil.GetConnectionString();
        }
        
        public async Task<ShoppingListEntity> CreateShoppingList(PostList list)
        {
            using var conn = connection;
            
            conn.Open();

            return await conn.QuerySingleAsync<ShoppingListEntity>(
                @"INSERT INTO lists (UserId, Name, Description) VALUES (@UserId, @Name, @Description);
                        SELECT * FROM lists WHERE Id = LAST_INSERT_ID()", list);
        }

        public async Task<ShoppingListEntity> GetListById(int id)
        {
            using var conn = connection;

            conn.Open();

            return await conn.QuerySingleAsync<ShoppingListEntity>(
               @"SELECT * FROM lists WHERE Id = @id", new { id });
        }
        
        public async Task<List<ShoppingListEntity>> GetListsByUserId(int id)
        {
            using var conn = connection;

            conn.Open();

            var lists = await conn.QueryAsync<ShoppingListEntity>(
                @"SELECT * FROM lists WHERE UserId = @id ORDER BY Id DESC", new { id });

            return lists.ToList();
        }

        public async Task<ShoppingListEntity> AddTagToList(int listId, ListTag tag)
        {
            using var conn = connection;
            
            conn.Open();

            var query = @"INSERT INTO listtags (ListId, TagId, Quantity, Comment) VALUES (@ListId, @TagId, @Quantity, @Comment);
                            SELECT * FROM lists WHERE Id = @ListId";

            return await conn.QuerySingleAsync<ShoppingListEntity>(query, new
            {
                ListId = listId, 
                TagId = tag.Tag.Id,
                Quantity = tag.Quantity,
                Comment = tag.Comment
            });
        }

        public async Task DeleteListTagAsync(int listId, int listTagId)
        {
            using var conn = connection;
            
            conn.Open();
            
            var query = @"DELETE FROM listtags WHERE ListId = @listId and Id = @listTagId";

            await conn.ExecuteAsync(query, new { listId, listTagId });
        }
    }
}
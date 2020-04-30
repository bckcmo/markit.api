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
        
        public async Task<ShoppingListEntity> CreateShoppingList(ShoppingList list)
        {
            using var conn = connection;
            
            conn.Open();

            var newList = await conn.QueryAsync<ShoppingListEntity>(
                @"INSERT INTO lists (UserId, Name, Description) VALUES (@UserId, @Name, @Description);
                        SELECT * FROM lists WHERE Id = LAST_INSERT_ID()", new
                {
                    UserId = list.UserId,
                    Name = list.Name,
                    Description = list.Description
                });
            
            var transaction = conn.BeginTransaction();
            
            var transformObject = list.ListTags.Select(t => new
            {
                ListId = newList.Single().Id,
                Comment = t.Comment,
                TagId = t.Tag.Id,
                Quantity = t.Quantity
            });
            
            await conn.ExecuteAsync(@"INSERT listtags(ListId, TagId, Quantity, Comment)
                VALUES (@ListId, @TagId, @Quantity, @Comment)", transformObject, transaction);
            
            transaction.Commit();
            return null;//result.Single();
        }
    }
}
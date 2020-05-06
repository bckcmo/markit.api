using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IListRepository
    {
        Task<ShoppingListEntity> CreateShoppingList(PostList list);
        Task<ShoppingListEntity> GetListById(int listId);
        Task<ShoppingListEntity> AddTagToList(int listId, ListTag tag);
        Task DeleteListTagAsync(int listId, int listTagId);
        Task<List<ShoppingListEntity>> GetListsByUserId(int id);
        Task<ListTagEntity> UpdateListTag(int listId, int listTagId, ListTag listTag);
        Task<ShoppingListEntity> UpdateList(int listId, PostList list);
        Task<ListTagEntity> CreateListTag(int listId, ListTag listTag);
        Task<ListTagEntity> GetListTag(int listTagId);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IListManager
    {
        Task<ShoppingList> CreateShoppingList(PostList list);
        Task<ShoppingList> GetListById(int listId);
        Task<List<ShoppingList>> GetListsByUserId(int userId);
        Task<ShoppingList> AddListTagToList(int listId, ListTag tag);
        Task DeleteListTagFromList(int listId, int listTagId);
    }
}
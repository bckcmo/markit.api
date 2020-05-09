using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IListManager
    {
        Task<ShoppingList> CreateList(PostList list);
        Task DeleteList(int id);
        Task<ShoppingList> GetListById(int listId);
        Task<List<ShoppingList>> GetListsByUserId(int userId);
        Task<ShoppingList> AddListTagToList(int listId, ListTag tag);
        Task DeleteListTagFromList(int listId, int listTagId);
        Task<ListTag> UpdateListTag(int listId, int listTagId, ListTag listTag);
        Task<ShoppingList> UpdateList(int listId, PostList list);
        Task<ListTag> CreateListTag(int listId, ListTag listTag);
    }
}
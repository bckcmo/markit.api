using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IListManager
    {
        Task<ShoppingList> CreateShoppingList(PostList list);
        Task<ShoppingList> GetListById(int listId);
        Task<ShoppingList> AddListTagToList(int listId, ListTag tag);
    }
}
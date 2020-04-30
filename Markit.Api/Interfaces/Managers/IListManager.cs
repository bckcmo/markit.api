using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IListManager
    {
        Task<ShoppingList> CreateShoppingList(ShoppingList list);
    }
}
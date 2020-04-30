using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IListRepository
    {
        Task<ShoppingListEntity> CreateShoppingList(ShoppingList list);
    }
}
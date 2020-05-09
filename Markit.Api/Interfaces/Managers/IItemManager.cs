using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IItemManager
    {
        Task<StoreItem> CreateStoreItemAsync(PostStoreItem item);
        Task<Item> GetItemByIdAsync(int id);
        Task<List<UserPrice>> QueryByCoordinatesAsync(decimal latitude, decimal longitude);
        Task DeleteItem(int id);
    }
}
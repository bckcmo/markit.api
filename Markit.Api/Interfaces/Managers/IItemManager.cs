using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Managers
{
    public interface IItemManager
    {
        Task<StoreItem> CreateStoreItemAsync(PostStoreItem item);
        Task<Item> GetItemByIdAsync(int id);
        Task<UserPriceList> GetUserPricesFromUserId(int userId);
        Task<UserPriceList> GetUserPricesFromStoreId(int userId);
        Task<List<UserPrice>> QueryByCoordinatesAsync(decimal latitude, decimal longitude);
        Task<UserPrice> GetUserPriceFromEntity(UserPriceEntity userPriceEntity);
        Task DeleteItem(int id);
    }
}
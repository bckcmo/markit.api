using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<ItemEntity> GetItemById(int id);
        Task<StoreItemEntity> GetStoreItemById(int id);
        Task<StoreItemEntity> GetStoreItemByItemId(int id);
        Task<ItemEntity> CreateItem(Item item);
        Task<StoreItemEntity> CreateStoreItem(PostStoreItem item);
        Task<List<UserPriceEntity>> GetUserPricesByStoreItemIds(List<int> storeItemIds);
        Task<List<UserPriceEntity>> GetUserPricesByUserId(int userId);
        Task<List<UserPriceEntity>> GetUserPricesByStoreId(int storeId);
        Task<UserPriceEntity> GetMostRecentUserPriceByTagName(string tagName, int storeId);
        Task<int> GetUserPriceCountByUserId(int userId);
        Task<int> GetUserPriceCountByStoreId(int storeId);
        Task DeleteItemById(int id);
    }
}
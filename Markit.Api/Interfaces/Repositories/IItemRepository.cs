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
        Task<List<UserPriceEntity>> GetUserPricesByItemId(int storeItemId);
        Task DeleteItemById(int id);
    }
}
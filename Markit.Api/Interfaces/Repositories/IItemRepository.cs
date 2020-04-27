using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<ItemEntity> GetItemByUpc(string upc);
        Task<ItemEntity> GetItemById(int id);
        Task<ItemEntity> CreateItem(Item item);
        Task<StoreItemEntity> CreateStoreItem(StoreItem item);
    }
}
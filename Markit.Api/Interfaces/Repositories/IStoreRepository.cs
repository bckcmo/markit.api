using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Repositories
{
    public interface IStoreRepository
    {
        Task<StoreEntity>CreateStore(Store store);
        Task<IList<StoreEntity>> QueryByCoordinates(decimal lat, decimal lon);
        Task<IList<StoreEntity>> QueryByCoordinates(decimal lat, decimal lon, int limit);
        Task<StoreEntity> GetStoreById(int id);
        Task<IList<StoreEntity>> GetStoresByIds(List<int> ids);
        Task<IList<StoreItemEntity>> GetStoreItemsFromStoreIds(List<int> storeIds);
        Task<StoreEntity> ReplaceStore(Store store);
        Task DeleteStoreById(int id);
    }
}
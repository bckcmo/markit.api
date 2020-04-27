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
        Task<IEnumerable<StoreEntity>> QueryByCoordinates(decimal lat, decimal lon);
        Task<StoreEntity> GetStoreById(int id);
        Task<StoreEntity> GetStoreById(int id, IDbConnection conn);
    }
}
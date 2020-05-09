using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Interfaces.Managers
{
    public interface IStoreManager
    {
        Task<Store> CreateStoreAsync(Store store);
        Task<IEnumerable<Store>> QueryByCoordinatesAsync(decimal lat, decimal lon);
        Task<Store> GetById(int id);
        Task Delete(int id);
        Task<Store> PutStore(Store store);
    }
}
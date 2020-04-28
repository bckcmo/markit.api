using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IItemManager
    {
        Task<StoreItem> CreateStoreItemAsync(PostStoreItem item);
        Task<StoreItem> GetStoreItemByIdAsync(int id);
    }
}
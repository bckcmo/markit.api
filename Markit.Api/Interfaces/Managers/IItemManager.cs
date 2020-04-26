using System.Threading.Tasks;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Interfaces.Managers
{
    public interface IItemManager
    {
        Task<StoreItem> CreateStoreItemAsync(StoreItem item);
    }
}
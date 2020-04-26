using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Managers
{
    public class ItemManager : IItemManager
    {
        private readonly IItemRepository _itemRepository;
        
        public ItemManager(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        
        public async Task<StoreItem> CreateStoreItemAsync(StoreItem item)
        {
            //check if item exists
            
            //if it doesn't, add item
            
            //add userPrice to item
            
            //add tags to item
            var itemEntity = new ItemEntity
            {
                
            };
            
            return item;
        }
    }
}
using System;
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
        
        public async Task<StoreItem> CreateStoreItemAsync(PostStoreItem item)
        {
            var newItem = await _itemRepository.CreateStoreItem(item);
   
            return new StoreItem
            {
                Id = newItem.Id,
                StoreId = newItem.StoreId,
                UserId = item.UserId,
                Upc = newItem.Upc,
                Price = item.Price,
                IsSalePrice = item.IsSalePrice
            };
        }
        
        public async Task<StoreItem> GetStoreItemByIdAsync(int id)
        {
            var storeItemEntity = await _itemRepository.GetStoreItemById(id);
            var userPriceEntity = await _itemRepository.GetUserPriceByItemId(storeItemEntity.Id);
            
            return new StoreItem
            {
                Id = storeItemEntity.Id,
                UserId = userPriceEntity.UserId,
                StoreId = storeItemEntity.StoreId,
                Upc = storeItemEntity.Upc,
                Price = userPriceEntity.Price,
                IsSalePrice = userPriceEntity.IsSalePrice
            };
        }
    }
}
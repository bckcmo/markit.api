using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        
        public ItemManager(IItemRepository itemRepository, IStoreRepository storeRepository, 
            IUserRepository userRepository, ITagRepository tagRepository)
        {
            _itemRepository = itemRepository;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
        }
        
        public async Task<StoreItem> CreateStoreItemAsync(PostStoreItem item)
        {
            var newItem = await _itemRepository.CreateStoreItem(item);
            await _userRepository.AddToReputation(item.UserId, 1);
            
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
        
        public async Task<Item> GetItemByIdAsync(int id)
        {
            var itemEntity = await _itemRepository.GetItemById(id);

            return new Item
            {
                Id = itemEntity.Id,
                Upc = itemEntity.Upc,
            };
        }

        public async Task DeleteItem(int id)
        {
            await _itemRepository.DeleteItemById(id);
        }

        public async Task<List<UserPrice>> QueryByCoordinatesAsync(decimal latitude, decimal longitude)
        {
            var storeEntities = await _storeRepository.QueryByCoordinates(latitude, longitude);
            var storeIds = storeEntities.Select(s => s.Id).ToList();
            var storeItemEntities = await _storeRepository.GetStoreItemsFromStoreIds(storeIds);
            var storeItemIds = storeItemEntities.Select(e => e.Id).ToList();
            var userPriceEntities = await _itemRepository.GetUserPricesByStoreItemIds(storeItemIds);

            return (await Task.WhenAll(userPriceEntities.Select(GetUserPriceFromEntity))).ToList();
        }

        public async Task<UserPrice> GetUserPriceFromEntity(UserPriceEntity userPriceEntity)
        {
            var storeItemEntity = await _itemRepository.GetStoreItemById(userPriceEntity.StoreItemId);
            var storeEntity = await _storeRepository.GetStoreById(storeItemEntity.StoreId);
            var itemEntity = await _itemRepository.GetItemById(storeItemEntity.ItemId);
            var tags = await _tagRepository.GetTagsByItemId(itemEntity.Id);
            var userEntity = await  _userRepository.GetById(userPriceEntity.UserId);
                
            return new UserPrice
            {
                Id = userPriceEntity.Id,
                UserName = userEntity.UserName,
                Store = new Store
                {
                    Id = storeEntity.Id,
                    Name = storeEntity.Name,
                    StreetAddress = storeEntity.StreetAddress,
                    City = storeEntity.City,
                    State = storeEntity.State,
                    PostalCode = storeEntity.PostalCode,
                    Coordinate = new Coordinate
                    {
                        Latitude = storeEntity.Latitude,
                        Longitude = storeEntity.Longitude
                    },
                    GoogleId = storeEntity.GoogleId
                },
                Item = new Item
                {
                    Id = itemEntity.Id,
                    Upc = itemEntity.Upc
                },
                Price = userPriceEntity.Price,
                TagNames = tags.Select(t => t.Name).ToList(),
                IsSalePrice = userPriceEntity.IsSalePrice,
                CreatedAt = userPriceEntity.CreatedAt
            };
        }
    }
}
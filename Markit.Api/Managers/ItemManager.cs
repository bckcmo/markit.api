using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        
        public ItemManager(IItemRepository itemRepository, IStoreRepository storeRepository, 
            IUserRepository userRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
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
            return _mapper.Map<Item>(itemEntity);
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

        public async Task<UserPriceList> GetUserPricesFromUserId(int userId)
        {
            var userPriceEntities = await _itemRepository.GetUserPricesByUserId(userId);
            var userPrices = (await Task.WhenAll(userPriceEntities.Select(GetUserPriceFromEntity)));
            return new UserPriceList
            {
                UserPrices = userPrices.ToList(),
                TotalRecords = await _itemRepository.GetUserPriceCountByUserId(userId)
            };
        }
        
        public async Task<UserPriceList> GetUserPricesFromStoreId(int storeId)
        {
            var userPriceEntities = await _itemRepository.GetUserPricesByStoreId(storeId);
            var userPrices = (await Task.WhenAll(userPriceEntities.Select(GetUserPriceFromEntity)));
            return new UserPriceList
            {
                UserPrices = userPrices.ToList(),
                TotalRecords = await _itemRepository.GetUserPriceCountByStoreId(storeId)
            };
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
                User = _mapper.Map<User>(userEntity),
                Store = _mapper.Map<Store>(storeEntity),
                Item = _mapper.Map<Item>(itemEntity),
                Price = userPriceEntity.Price,
                TagNames = tags.Select(t => t.Name).ToList(),
                IsSalePrice = userPriceEntity.IsSalePrice,
                CreatedAt = userPriceEntity.CreatedAt
            };
        }
    }
}
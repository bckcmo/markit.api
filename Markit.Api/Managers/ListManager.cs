using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Markit.Api.Exceptions;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Managers
{
    public class ListManager : IListManager
    {
        private readonly IListRepository _listRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemManager _itemManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public ListManager(IListRepository listRepository, ITagRepository tagRepository, 
            IStoreRepository storeRepository, IItemRepository itemRepository, 
            IItemManager itemManager, IUserRepository userRepository, IMapper mapper)
        {
            _listRepository = listRepository;
            _tagRepository = tagRepository;
            _storeRepository = storeRepository;
            _itemRepository = itemRepository;
            _itemManager = itemManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<ShoppingList> CreateList(PostList list)
        {
            var listEntity = await _listRepository.CreateShoppingList(list);
            var newList = _mapper.Map<ShoppingList>(listEntity);
            return newList;
        }

        public async Task DeleteList(int id)
        {
            await _listRepository.DeleteList(id);
        }

        public async Task<ShoppingList> GetListById(int listId)
        {
            var list = await _listRepository.GetListById(listId);
            return await BuildShoppingListFromEntity(list, listId);
        }

        public async Task<ShoppingList> AddListTagToList(int listId, ListTag tag)
        {
            var listEntity = await _listRepository.AddTagToList(listId, tag);
            return await BuildShoppingListFromEntity(listEntity, listId);
        }

        public async Task DeleteListTagFromList(int listId, int listTagId)
        {
            await _listRepository.DeleteListTagAsync(listId, listTagId);
        }

        public async Task<List<ShoppingList>> GetListsByUserId(int userId)
        {
            var listEntities = await _listRepository.GetListsByUserId(userId);
            var lists = (await Task.WhenAll(
                listEntities.Select(l => BuildShoppingListFromEntity(l, l.Id))))
                .ToList();

            return lists;
        }

        public async Task<ListTag>CreateListTag(int listId, ListTag listTag)
        {
            var tagEntity = await _tagRepository.GetTagById(listTag.Tag.Id);
            var newListTag = await _listRepository.CreateListTag(listId, listTag);
            return BuildListTagFromEntity(newListTag, tagEntity);
        }
        
        public async Task<ListTag> UpdateListTag(int listId, int listTagId, ListTag listTag)
        {
            //TODO wrap in custom exception and handle error if tag doesn't exist
            var listTagEntity = await _listRepository.GetListTag(listTagId);
            var updatedListTag = await _listRepository.UpdateListTag(listId, listTagId, listTag);
            var tagEntity = await _tagRepository.GetTagById(updatedListTag.TagId);
            return BuildListTagFromEntity(updatedListTag, tagEntity);
        }

        public async Task<ShoppingList> UpdateList(int listId, PostList list)
        {
            var updatedList = await _listRepository.UpdateList(listId, list);
            return await BuildShoppingListFromEntity(updatedList, listId);
        }

        public async Task<ListAnalysis> AnalyzeList(ShoppingList list, decimal latitude, decimal longitude)
        {
            var nearbyStores = (await _storeRepository.QueryByCoordinates(latitude, longitude)).ToList();

            if (!nearbyStores.Any())
            {
                throw new ListAnalysisException("Not enough nearby stores to complete the analysis.");
            }
            
            var listAnalysis = new ListAnalysis {Rankings = new List<StoreAnalysis>()};
            
            foreach (var store in nearbyStores)
            {
                listAnalysis.Rankings.Add(await BuildStoreAnalysisFromStoreEntity(store, list));
            }

            AddRankingsToList(listAnalysis);
            
            return listAnalysis;
        }

        private void AddRankingsToList(ListAnalysis listAnalysis)
        {
            int priceRank = 0, stalnessRank = 0, totalRank = 0;

            listAnalysis.Rankings = listAnalysis.Rankings.OrderBy(r => r.TotalPrice)
                .Select(analysis => {
                    analysis.PriceRank = analysis.MissingItems ? -1 : ++priceRank;
                    return analysis;
                }).OrderBy(r => r.Staleness)
                .Select(analysis => {
                    analysis.StalenessRank = analysis.MissingItems ? -1 : ++stalnessRank;
                    return analysis; 
                }).OrderBy(r => (r.StalenessRank * 0.333 ) + ( r.PriceRank * 0.6666))
                .Select(analysis => {
                    analysis.PriceAndStalenessRank = analysis.MissingItems ? -1 : ++totalRank;
                    return analysis; 
                }).ToList();
        }

        private async Task<StoreAnalysis> BuildStoreAnalysisFromStoreEntity(StoreEntity storeEntity, ShoppingList list)
        {
            var userPrices = await GetUserPricesFromList(list, storeEntity);
            
            var storeAnalysis = new StoreAnalysis {ListItems = new List<ListAnalysisItem>()};

            foreach (var userPrice in userPrices)
            {
                if (userPrice == null)
                {
                    storeAnalysis.MissingItems = true;
                    continue;
                }
                
                var quantity = list.ListTags.FirstOrDefault(t => userPrice.TagNames.Contains(t.Tag.Name))?.Quantity;
                quantity ??= 1;
                storeAnalysis.TotalPrice += (userPrice.Price * (decimal) quantity);
                storeAnalysis.Staleness += AssignStalenessToPrice(userPrice);
                storeAnalysis.ListItems.Add(await MapUserPriceToListItem(userPrice));
            }

            storeAnalysis.Store = _mapper.Map<Store>(storeEntity);

            return storeAnalysis;
        }

        private async Task<List<UserPriceEntity>> GetUserPriceEntities(ShoppingList list, StoreEntity storeEntity)
        {
            var userPriceEntities =  await Task.WhenAll(list.ListTags.Select(async t =>
            {
                try
                {
                    return await _itemRepository.GetMostRecentUserPriceByTagName(t.Tag.Name, storeEntity.Id);
                }
                catch
                {
                    return null;
                }
            }));

            return userPriceEntities.ToList();
        }

        private async Task<List<UserPrice>> GetUserPricesFromList(ShoppingList list, StoreEntity storeEntity)
        {
            var userPrices = await Task.WhenAll((await GetUserPriceEntities(list, storeEntity)).Select(async e => {
                try
                {
                    return await _itemManager.GetUserPriceFromEntity(e);
                }
                catch
                { 
                    return null;
                }
            }));

            return userPrices.ToList();
        }

        private async Task<ListAnalysisItem> MapUserPriceToListItem(UserPrice userPrice)
        {
            var userEntity = await _userRepository.GetByUserName(userPrice.UserName);
            var user = _mapper.Map<User>(userEntity);
            
            return new ListAnalysisItem
            {
                Item = userPrice.Item,
                Price = userPrice.Price,
                IsSalePrice = userPrice.IsSalePrice,
                TagNames = userPrice.TagNames,
                PriceSubmittedBy = new PriceAttribution
                {
                    UserName = user.UserName,
                    UserReputation = user.Reputation,
                    UserLevel = user.GetUserLevel()
                }
            };
        }

        private async Task<ShoppingList> BuildShoppingListFromEntity(ShoppingListEntity entity, int listId)
        {
            var listTagEntities = await _tagRepository.GetListTags(listId);

            var listTags = (await Task.WhenAll(listTagEntities.Select( async t =>
            {
                var tagEntity = await _tagRepository.GetTagById(t.TagId);
                return BuildListTagFromEntity(t, tagEntity);
            }))).ToList();

            var shoppingList = _mapper.Map<ShoppingList>(entity);
            shoppingList.ListTags = listTags;

            return shoppingList;
        }

        private ListTag BuildListTagFromEntity(ListTagEntity listTagEntity, TagEntity tagEntity)
        {
            var listTag = _mapper.Map<ListTag>(listTagEntity);
            listTag.Tag.Name = tagEntity.Name;
            return listTag;
        }

        private int AssignStalenessToPrice(UserPrice price)
        {
            var currentDate = DateTime.Now;
            var staleness = 0;
            
            if (price.CreatedAt <= currentDate.AddDays(-1) && price.CreatedAt >= currentDate.AddDays(-7))
            {
                staleness = 1;
            } 
            else if (price.CreatedAt < currentDate.AddDays(-7) && price.CreatedAt >= currentDate.AddDays(-14))
            {
                staleness = 2;
            }
            else if(price.CreatedAt < currentDate.AddDays(-14))
            {
                staleness = 3;
            }

            var salePriceMultiplier = price.IsSalePrice ? 2 : 1;

            return staleness * salePriceMultiplier;
        }
    }
}
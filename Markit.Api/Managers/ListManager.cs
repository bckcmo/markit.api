using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Managers
{
    public class ListManager : IListManager
    {
        private readonly IListRepository _listRepository;
        private readonly ITagRepository _tagRepository;
        
        public ListManager(IListRepository listRepository, ITagRepository tagRepository)
        {
            _listRepository = listRepository;
            _tagRepository = tagRepository;
        }
        public async Task<ShoppingList> CreateShoppingList(PostList list)
        {
            var newList = await _listRepository.CreateShoppingList(list);
            
            return new ShoppingList
            {
                Id = newList.Id,
                UserId = list.UserId,
                Description = list.Description,
                Name = list.Name
            };
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

        private async Task<ShoppingList> BuildShoppingListFromEntity(ShoppingListEntity entity, int listId)
        {
            var listTagEntities = await _tagRepository.GetListTags(listId);

            var listTags = (await Task.WhenAll(listTagEntities.Select( async t =>
            {
                var tag = await _tagRepository.GetTagById(t.TagId);
                
                return new ListTag
                {
                    Id = t.Id,
                    Tag = new Tag
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    },
                    Quantity = t.Quantity,
                    Comment = t.Comment
                };
            }))).ToList();
            
            return new ShoppingList
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                Description = entity.Description,
                ListTags = listTags
            };
        }
    }
}
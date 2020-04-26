using System;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;

namespace Markit.Api.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public Task<ItemEntity> GetItemByUpc(string upc)
        {
            throw new NotImplementedException();
        }

        public Task<ItemEntity> GetItemById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ItemEntity> CreateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
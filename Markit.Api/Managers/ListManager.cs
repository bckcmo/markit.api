using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Models.Dtos;

namespace Markit.Api.Managers
{
    public class ListManager : IListManager
    {
        private readonly IListRepository _listRepository;
        
        public ListManager(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }
        public async Task<ShoppingList> CreateShoppingList(ShoppingList list)
        {
            var newList = await _listRepository.CreateShoppingList(list);
            
            return new ShoppingList
            {
                
            };
        }
    }
}
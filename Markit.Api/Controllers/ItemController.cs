using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [Authorize]
    [ApiController, Route("item")]
    public class ItemController : Controller
    {
        private readonly IItemManager _itemManager;
        public ItemController(IItemManager itemManager)
        {
            _itemManager = itemManager;
        }
        
        [AllowAnonymous]
        [HttpGet("{itemId}")]
        public IActionResult Get(int itemId)
        {
            return Ok(new Item
            {
                Id = itemId,
                Upc = "839472834759",
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(StoreItem item)
        {
            var newItem = await _itemManager.CreateStoreItemAsync(item);
            return Ok(item);
        }

        [HttpDelete("{itemId}")]
        public IActionResult Delete(int itemId)
        {
            return Ok();
        }
        
        // TODO add GET item/prices endpoint that gets most recent prices
    }
}
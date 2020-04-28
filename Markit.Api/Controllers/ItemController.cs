using System.Threading.Tasks;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [Authorize]
    [ApiController, Route("item")]
    public class ItemController : Controller
    {
        private readonly IItemManager _itemManager;
        private readonly IHttpContextAccessor _httpContext;
        
        public ItemController(IItemManager itemManager, IHttpContextAccessor httpContext)
        {
            _itemManager = itemManager;
            _httpContext = httpContext;
        }
        
        [AllowAnonymous]
        [HttpGet("{itemId}")]
        public async Task<IActionResult> Get(int itemId)
        {
            var item = await _itemManager.GetStoreItemByIdAsync(itemId);
            return Ok(item);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(PostStoreItem item)
        {
            if (!_httpContext.IsUserAllowed(item.UserId))
            {
                return Unauthorized();
            }
            
            var newItem = await _itemManager.CreateStoreItemAsync(item);
            item.Id = newItem.Id;
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
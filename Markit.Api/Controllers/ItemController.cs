using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Statics;
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
            var item = await _itemManager.GetItemByIdAsync(itemId);
            return Ok(new MarkitApiResponse {Data = item});
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(PostStoreItem item)
        {
            if (!_httpContext.IsUserAllowed(item.UserId))
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> { ErrorMessages.UserDenied }
                });
            }
            
            var newItem = await _itemManager.CreateStoreItemAsync(item);
            item.Id = newItem.Id;
            return Ok(new MarkitApiResponse { Data = item });
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            if (!_httpContext.IsSuperUser())
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> { ErrorMessages.MissingPrivileges }
                });
            }
            
            await _itemManager.DeleteItem(itemId);
            return Ok(new MarkitApiResponse());
        }
        
        [AllowAnonymous]
        [HttpGet("/prices/query")]
        public async Task<IActionResult> Get([FromQuery] decimal latitude, [FromQuery] decimal longitude)
        {
            var item = await _itemManager.QueryByCoordinatesAsync(latitude, longitude);
            return Ok(new MarkitApiResponse {Data = item});
        }
    }
}
using System.Collections.Generic;
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
     [ApiController, Route("list")]
     public class ListController : Controller
     {
         private readonly IListManager _listManager;
         private readonly IHttpContextAccessor _httpContext;
         public ListController(IListManager listManager, IHttpContextAccessor httpContext)
         {
             _listManager = listManager;
             _httpContext = httpContext;
         }
         
         [HttpGet("{listId}")]
         public async Task<IActionResult> Get(int listId)
         {
             var list = await _listManager.GetListById(listId);
             
             if (!_httpContext.IsUserAllowed(list.UserId))
             {
                 return Unauthorized();
             }
             
             return Ok(list);
         }
         
         [HttpPost]
         public async Task<IActionResult> Post(PostList list)
         {
             if (!_httpContext.IsUserAllowed(list.UserId))
             {
                 return Unauthorized();
             }
             
             var newList = await _listManager.CreateShoppingList(list);
             return Ok(newList);
         }

         [HttpPatch("{listId}")]
         public async Task<IActionResult> Patch(int listId, ListTag tag)
         {
             var list = await _listManager.GetListById(listId);
             
             if (!_httpContext.IsUserAllowed(list.UserId))
             {
                 return Unauthorized();
             }
             
             var updatedList = await _listManager.AddListTagToList(listId, tag);

             return Ok(updatedList);
         }
         
         [HttpDelete("{listId}")]
         public IActionResult Delete(string listId)
         {
             return Ok();
         }
         
         [HttpDelete("{listId}/listTag/{listTagId}")]
         public IActionResult DeleteListTag(string listId)
         {
             return Ok();
         }
     }
 }
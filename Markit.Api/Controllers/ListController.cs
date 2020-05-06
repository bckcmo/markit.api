using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Entities;
using Markit.Api.Models.Statics;
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
                 return Unauthorized(new MarkitApiResponse
                 {
                     StatusCode = StatusCodes.Status401Unauthorized,
                     Errors = new List<string> { ErrorMessages.UserDenied }
                 });
             }
             
             return Ok(new MarkitApiResponse { Data = list });
         }
         
         [HttpPost]
         public async Task<IActionResult> Post(PostList list)
         {
             if (!_httpContext.IsUserAllowed(list.UserId))
             {
                 return Unauthorized(new MarkitApiResponse
                 {
                     StatusCode = StatusCodes.Status401Unauthorized,
                     Errors = new List<string> { ErrorMessages.UserDenied }
                 });
             }
             
             var newList = await _listManager.CreateShoppingList(list);
             return Ok(new MarkitApiResponse { Data = newList });
         }

         [HttpPatch("{listId}")]
         public async Task<IActionResult> Patch(int listId, PostList postList)
         {
             var list = await _listManager.GetListById(listId);
             
             if (!_httpContext.IsUserAllowed(list.UserId))
             {
                 return Unauthorized(new MarkitApiResponse
                 {
                     StatusCode = StatusCodes.Status401Unauthorized,
                     Errors = new List<string> { ErrorMessages.UserDenied }
                 });
             }
             
             var updatedList = await _listManager.UpdateList(listId, postList);

             return Ok(new MarkitApiResponse { Data = updatedList });
         }
         
         [HttpPatch("{listId}/listTag/{listTagId}")]
         public async Task<IActionResult> Patch(int listId, int listTagId, ListTag tag)
         {
             var list = await _listManager.GetListById(listId);
             
             if (!_httpContext.IsUserAllowed(list.UserId))
             {
                 return Unauthorized(new MarkitApiResponse
                 {
                     StatusCode = StatusCodes.Status401Unauthorized,
                     Errors = new List<string> { ErrorMessages.UserDenied }
                 });
             }
             
             var updatedListTag = await _listManager.UpdateListTag(listId, listTagId, tag);

             return Ok(new MarkitApiResponse { Data = updatedListTag });
         }
         
         [HttpDelete("{listId}")]
         public IActionResult Delete(string listId)
         {
             return Ok();
         }
         
         [HttpDelete("{listId}/listTag/{listTagId}")]
         public async Task<IActionResult> DeleteListTag(int listId, int listTagId)
         {
             await _listManager.DeleteListTagFromList(listId, listTagId);
             return Ok(new MarkitApiResponse());
         }
     }
 }
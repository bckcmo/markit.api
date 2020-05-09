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
     [ApiController, Route("store")]
     public class StoreController : Controller
     {
         private readonly IStoreManager _storeManager;
         private readonly IHttpContextAccessor _httpContext;

         public StoreController(IHttpContextAccessor httpContext, IStoreManager storeManager)
         {
             _storeManager = storeManager;
             _httpContext = httpContext;
         }
         
         [HttpGet("{storeId}")]
         public async Task<IActionResult> Get(int storeId)
         {
             var store = await _storeManager.GetById(storeId);
             return Ok( new MarkitApiResponse { Data = store });
         }
         
         [HttpGet("query")]
         public async Task<IActionResult> Get([FromQuery] decimal latitude, [FromQuery] decimal longitude)
         {
             var stores = await _storeManager.QueryByCoordinatesAsync(latitude, longitude);
             return Ok( new MarkitApiResponse { Data = stores });
         }
         
         [Authorize]
         [HttpPost]
         public async Task<IActionResult> Post(Store store)
         {
             var newStore = await _storeManager.CreateStoreAsync(store);
             return Ok( new MarkitApiResponse { Data = newStore });
         }
         
         [Authorize]
         [HttpPut]
         public async Task<IActionResult> Put(Store store)
         {
             if (!_httpContext.IsSuperUser())
             {
                 return Unauthorized(new MarkitApiResponse
                 {
                     StatusCode = StatusCodes.Status401Unauthorized,
                     Errors = new List<string> { ErrorMessages.MissingPrivileges }
                 });
             }
             
             var updatedStore = await _storeManager.PutStore(store);
             return Ok(new MarkitApiResponse { Data = updatedStore });
         }
         
         [Authorize]
         [HttpDelete("{storeId}")]
         public async Task<IActionResult> Delete(int storeId)
         {
             if (!_httpContext.IsSuperUser())
             {
                 return Unauthorized(new MarkitApiResponse
                 {
                     StatusCode = StatusCodes.Status401Unauthorized,
                     Errors = new List<string> { ErrorMessages.MissingPrivileges }
                 });
             }
             
             await _storeManager.Delete(storeId);
             return Ok();
         }
     }
 }
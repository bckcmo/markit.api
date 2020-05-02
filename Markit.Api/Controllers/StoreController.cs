using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
 {
     [ApiController, Route("store")]
     public class StoreController : Controller
     {
         private readonly IStoreManager _storeManager;

         public StoreController(IStoreManager storeManager)
         {
             _storeManager = storeManager;
         }
         
         [HttpGet("{storeId}")]
         public IActionResult Get(int storeId)
         {
            return Ok( new MarkitApiResponse {
                Data = new Store
                {
                   Id = storeId,
                   Name ="Food 'n Stuff",
                   StreetAddress = "101 Main St.",
                   City = "Pawnee",
                   State = "IN"
                }
            });
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
         public IActionResult Put(Store store)
         {
             return Ok(new MarkitApiResponse { Data = store });
         }
         
         [Authorize]
         [HttpDelete("{storeId}")]
         public IActionResult Delete(string storeId)
         {
             return Ok();
         }
     }
 }
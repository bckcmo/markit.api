using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
 {
     [ApiController, Route("store")]
     public class StoreController : Controller
     {
         [HttpGet("{storeId}")]
         public IActionResult Get(int storeId)
         {
            return Ok(new Store
            {
               Id = storeId,
               Name ="Food 'n Stuff",
               StreetAddress = "101 Main St.",
               City = "Pawnee",
               State = "IN"
            });
         }
         
         [HttpPost]
         public IActionResult Post(Store store)
         {
             return Ok(store);
         }
         
         [HttpPut]
         public IActionResult Put(Store store)
         {
             return Ok(store);
         }
         
         [HttpDelete("{storeId}")]
         public IActionResult Delete(string storeId)
         {
             return Ok();
         }
     }
 }
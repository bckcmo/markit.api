using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
 {
     [ApiController, Route("store")]
     public class StoreController : Controller
     {
         [HttpGet("{storeId}")]
         public IActionResult Get(string storeId)
         {
             return Ok($"storeId: {storeId}");
         }
         
         [HttpPost]
         public IActionResult Post()
         {
             return Ok("test");
         }
         
         [HttpPatch("{storeId}")]
         public IActionResult Patch(string storeId)
         {
             return Ok($"storeId: {storeId}");
         }
         
         [HttpDelete("{storeId}")]
         public IActionResult Delete(string storeId)
         {
             return Ok($"storeId: {storeId}");
         }
     }
 }
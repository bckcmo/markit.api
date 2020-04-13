using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
 {
     [ApiController, Route("list")]
     public class ListController : Controller
     {
         [HttpGet("{listId}")]
         public IActionResult Get(string userId, string listId)
         {
             return Ok($"listId: {listId}, userId: {userId}");
         }
         
         [HttpPost]
         public IActionResult Post()
         {
             return Ok("test");
         }
         
         [HttpPatch("{listId}")]
         public IActionResult Patch(string listId, string tag)
         {
             return Ok($"listId: {listId}, tag: {tag}");
         }
         
         [HttpDelete("{listId}")]
         public IActionResult Delete(string listId)
         {
             return Ok($"listId: {listId}");
         }
     }
 }
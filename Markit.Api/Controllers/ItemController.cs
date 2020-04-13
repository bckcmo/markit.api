using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("item")]
    public class ItemController : Controller
    {
        [HttpGet("{listId}")]
        public IActionResult Get(string userId, string listId)
        {
            return Ok($"itemId: {listId}");
        }
        
        [HttpPost]
        public IActionResult Post()
        {
            return Ok("item");
        }

        [HttpDelete("{listId}")]
        public IActionResult Delete(string listId)
        {
            return Ok($"itemId: {listId}");
        }
    }
}
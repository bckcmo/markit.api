using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("item")]
    public class ItemController : Controller
    {
        [HttpGet("{itemId}")]
        public IActionResult Get(string itemId)
        {
            return Ok($"itemId: {itemId}");
        }
        
        [HttpPost]
        public IActionResult Post()
        {
            return Ok("item");
        }

        [HttpDelete("{itemId}")]
        public IActionResult Delete(string itemId)
        {
            return Ok($"itemId: {itemId}");
        }
    }
}
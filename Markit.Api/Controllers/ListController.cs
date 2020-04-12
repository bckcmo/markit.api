using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    public class ListController : Controller
    {
        [HttpGet("list/{listId}")]
        public IActionResult Get(string userId, string listId)
        {
            return Ok($"listId: {listId}, userId: {userId}");
        }
        
        [HttpPost("list")]
        public IActionResult Post()
        {
            return Ok("test");
        }
        
        [HttpPatch("list/{listId}")]
        public IActionResult Patch(string listId, string tag)
        {
            return Ok($"listId: {listId}, tag: {tag}");
        }
        
        [HttpDelete("list/{listId}")]
        public IActionResult Delete(string listId)
        {
            return Ok($"listId: {listId}");
        }
    }
}
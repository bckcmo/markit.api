using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("tags")]
    public class TagController : Controller
    {
        [HttpGet]
        public IActionResult Query([FromQuery] string tagQuery)
        {
            return Ok(tagQuery);
        }
    }
}
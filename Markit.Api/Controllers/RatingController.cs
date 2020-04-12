using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    public class RatingController : Controller
    {
        [HttpPost("rating")]
        public IActionResult Post()
        {
            return Ok("placeholder");
        }
        
        [HttpGet("ratings")]
        public IActionResult GetAll([FromQuery] decimal latitude, [FromQuery] decimal longitude)
        {
            return Ok($"Latitude: {latitude}, Longitude: {longitude}");
        }
    }
}
using System.Collections.Generic;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController]
    public class RatingController : Controller
    {
        [HttpPost("rating")]
        public IActionResult Post(Rating rating)
        {
            return Ok(rating);
        }
        
        [HttpGet("rating/query")]
        public IActionResult Get([FromQuery] decimal latitude, [FromQuery] decimal longitude)
        {
            return Ok(new List<Rating>
            {
                new Rating
                {
                    Id = 1,
                    Store = new Store
                    {
                       Id = 0,
                       Name ="Food 'n Stuff",
                       StreetAddress = "101 Main St.",
                       City = "Pawnee",
                       State = "IN",
                       Coordinate = new Coordinate
                       {
                           Latitude = latitude,
                           Longitude = longitude
                       }
                    },
                    Comment = "Great store. It's where I get all my food. And most of my stuff.",
                    Points = 5
                }
            });
        }
        
        // TODO add get ratings endpoint that returns most recent ratings
    }
}
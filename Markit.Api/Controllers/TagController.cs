using System.Collections.Generic;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("tags")]
    public class TagController : Controller
    {
        [HttpGet("query")]
        public IActionResult Query([FromQuery] string tagQuery)
        {
            return Ok(new List<Tag>
            {
                new Tag
                {
                    Id = 0,
                    Name = "Prego Tomato Sauce"
                },
                new Tag
                {
                    Id = 1,
                    Name = "Prego Alfredo Sauce"
                }
            });
        }
    }
}
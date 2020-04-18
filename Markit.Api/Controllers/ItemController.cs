using System;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("item")]
    public class ItemController : Controller
    {
        [HttpGet("{itemId}")]
        public IActionResult Get(int itemId)
        {
            return Ok(new Item
            {
                Id = itemId,
                Upc = "839472834759",
            });
        }
        
        [HttpPost]
        public IActionResult Post(StoreItem item)
        {
            return Ok(item);
        }

        [HttpDelete("{itemId}")]
        public IActionResult Delete(int itemId)
        {
            return Ok();
        }
    }
}
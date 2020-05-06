using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("tags")]
    public class TagController : Controller
    {
        private readonly ITagManager _tagManager;
        public TagController(ITagManager tagManager)
        {
            _tagManager = tagManager;
        }
        
        [HttpGet("query")]
        public async Task<IActionResult> Query([FromQuery] string name, [FromQuery] string upc)
        {
            var tags = await _tagManager.QueryTagsAsync(name, upc, 10);
            
            return Ok( new MarkitApiResponse { Data = tags });
        }
    }
}
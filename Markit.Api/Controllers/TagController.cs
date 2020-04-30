﻿using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
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
        public async Task<IActionResult> Query([FromQuery] string tagQuery)
        {
            var tags = await _tagManager.QueryTagsAsync(tagQuery, 10);
            
            return Ok(tags);
        }
    }
}
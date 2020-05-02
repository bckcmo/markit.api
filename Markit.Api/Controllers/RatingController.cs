using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController]
    public class RatingController : Controller
    {
        private readonly IRatingsManager _ratingsManager;
        private readonly IHttpContextAccessor _httpContext;

        public RatingController(IRatingsManager ratingsManager, IHttpContextAccessor httpContext)
        {
            _ratingsManager = ratingsManager;
            _httpContext = httpContext;
        }
        
        [Authorize]
        [HttpPost("rating")]
        public async Task<IActionResult> Post(Rating rating)
        {
            if (!_httpContext.IsUserAllowed(rating.UserId))
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> { ErrorMessages.UserDenied }
                });
            }
            
            var newRating = await _ratingsManager.CreateRating(rating);
            
            return Ok( new MarkitApiResponse { Data = newRating });
        }
        
        [HttpGet("ratings/query")]
        public async Task<IActionResult> Query([FromQuery] decimal latitude, [FromQuery] decimal longitude)
        {
           var ratings = await _ratingsManager.QueryByCoordinatesAsync(latitude, longitude);
           return Ok(new MarkitApiResponse { Data = ratings });
        }
    }
}
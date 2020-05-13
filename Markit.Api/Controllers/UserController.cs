using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [Authorize]
    [ApiController, Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IListManager _listManager;
        private readonly IHttpContextAccessor _httpContext;

        public UserController(IUserManager userManager, IListManager listManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _listManager = listManager;
            _httpContext = httpContext;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            if (!_httpContext.IsUserAllowed(userId))
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> {ErrorMessages.UserDenied}
                });
            }

            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new List<string> {ErrorMessages.ResourceNotFound}
                });
            }

            return Ok(new MarkitApiResponse {Data = user});
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserByIdAsync(_httpContext.GetCurrentUserId());

            if (user == null)
            {
                return NotFound(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new List<string> {ErrorMessages.ResourceNotFound}
                });
            }

            return Ok(new MarkitApiResponse {Data = user});
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(UserRegistration user)
        {
            var newUser = await _userManager.CreateUserAsync(user);
            return Ok(new MarkitApiResponse {Data = newUser});
        }

        [HttpPut]
        public async Task<IActionResult> Put(User user)
        {
            if (!_httpContext.IsUserAllowed(user.Id))
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> {ErrorMessages.UserDenied}
                });
            }

            var userResponse = await _userManager.UpdateUserAsync(user);

            if (user.Id == 0)
            {
                return NotFound(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new List<string> {ErrorMessages.ResourceNotFound}
                });
            }

            return Ok(new MarkitApiResponse {Data = userResponse});
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            await _userManager.DeleteUserAsync(userId);
            return Ok(new MarkitApiResponse());
        }

        [HttpGet("{userId}/lists")]
        public async Task<IActionResult> GetAll(int userId)
        {
            if (!_httpContext.IsUserAllowed(userId))
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> {ErrorMessages.UserDenied}
                });
            }

            var lists = await _listManager.GetListsByUserId(userId);

            return Ok(new MarkitApiResponse {Data = lists});
        }

        [HttpGet("{userId}/list/{listId}/analyze")]
        public async Task<IActionResult> Get(int userId, int listId, 
            [Required] decimal latitude, 
            [Required] decimal longitude)
        {
            if (!_httpContext.IsUserAllowed(userId))
            {
                return Unauthorized(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = new List<string> { ErrorMessages.UserDenied }
                });
            }

            try
            {
                var list = await _listManager.GetListById(listId);
                //var listAnalysis = await _listManager.AnalyzeList(list, latitude, longitude);
            }
            catch (Exception exception)
            {
                return NotFound(new MarkitApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new List<string> { ErrorMessages.ResourceNotFound }
                });
            }

            return Ok( new MarkitApiResponse { Data = new ListAnalysis
            {
                Rankings = new List<StoreAnalysis>
                {
                    new StoreAnalysis
                    {
                        Store = new Store
                        {
                            Id = 0,
                            Name ="Food 'n Stuff",
                            StreetAddress = "101 Main St.",
                            City = "Pawnee",
                            State = "IN"
                        },
                        TotalPrice = 67.88m,
                        Staleness = 10,
                        PriceRank = 1,
                        PriceAndStalenessRank = 2
                    },
                    new StoreAnalysis
                    {
                        Store = new Store
                        {
                            Id = 1,
                            Name ="Woody's Discount Grocer",
                            StreetAddress = "400 West Lane",
                            City = "Pawnee",
                            State = "IN"
                        },
                        TotalPrice = 72.14m,
                        Staleness = 0,
                        PriceRank = 2,
                        PriceAndStalenessRank = 1
                    }
                }
            }});
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Post(UserAuth authRequest)
        {
            var token = await _userManager.GenerateToken(authRequest);
            return Ok(new MarkitApiResponse { Data = token });
        }
    }
}
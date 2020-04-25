using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Extensions;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models;
using Markit.Api.Models.Dtos;
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
        private readonly IHttpContextAccessor _httpContext;

        public UserController(IUserManager userManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            this._httpContext = httpContext;
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            if (!_httpContext.IsUserAllowed(userId))
            {
                return Unauthorized();
            }
            
            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(UserRegistration user)
        {
            var newUser = await _userManager.CreateUserAsync(user);
            return Ok(newUser);
        }

        [HttpPut]
        public async Task<IActionResult> Put(User user)
        {
            if (!_httpContext.IsUserAllowed(user.Id))
            {
                return Unauthorized();
            }
            
            var userResponse = await _userManager.UpdateUserAsync(user);

            if (user.Id == 0)
            {
                return NotFound();
            }
            
            return Ok(userResponse);
        }
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
        
        [HttpGet("{userId}/lists")]
        public IActionResult GetAll(int userId)
        {
            return Ok(new UserShoppingLists
            {
                UserId = userId,
                Lists = new List<ShoppingList>
                {
                    new ShoppingList
                    {
                        Id = 1,
                        Name = "Test List",
                        Description = "This is a hardcoded list",
                        ListTags = new List<ListTag>
                        {
                            new ListTag
                            {
                                Id = 0,
                                Tag = new Tag
                                {
                                  Id = 0,
                                  Name = "Cat food"
                                },
                                Quantity = 1,
                                Comment = "Meow Meow"
                            },
                            new ListTag
                            {
                                Id = 1,
                                Tag = new Tag
                                {
                                    Id = 20,
                                    Name = "Doritos"
                                },
                                Quantity = 1,
                            },
                            new ListTag
                            {
                                Id = 2,
                                Tag = new Tag
                                {
                                    Id = 6,
                                    Name = "Bread"
                                },
                                Quantity = 1,
                            }
                        },
                    },
                    new ShoppingList
                    {
                        Id = 1,
                        Name = "Test List 2",
                        Description = "This is also a hardcoded list",
                        ListTags = new List<ListTag>
                        {
                            new ListTag
                            {
                                Id = 0,
                                Tag = new Tag
                                {
                                    Id = 0,
                                    Name = "Dog food"
                                },
                                Quantity = 1,
                                Comment = "Woof Woof"
                            },
                            new ListTag
                            {
                                Id = 1,
                                Tag = new Tag
                                {
                                    Id = 20,
                                    Name = "Cheese"
                                },
                                Quantity = 2,
                                Comment = "Get colby jack and cheddar"
                            },
                            new ListTag
                            {
                                Id = 2,
                                Tag = new Tag
                                {
                                    Id = 6,
                                    Name = "Milk"
                                },
                                Quantity = 1,
                            }
                        }
                    }
                }
            });
        }
        
        [HttpGet("{userId}/list/{listId}/analyze")]
        public IActionResult Get(int userId, int listId)
        {
            return Ok(new ListAnalysis
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
            });
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Post(UserAuth authRequest)
        {
            var token = await _userManager.GenerateToken(authRequest);
            return Ok(token);
        }
    }
}
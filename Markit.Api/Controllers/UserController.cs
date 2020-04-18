using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markit.Api.Models;
using Markit.Api.Models.Dtos;
using Markit.Api.Models.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Markit.Api.Controllers
{
    [ApiController, Route("user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return Ok( new User
            {
               Id = userId,
               FirstName = "TestFirstName",
               LastName = "TestLastName",
               Email = "Test@example.com",
               UserName = "TEsTUseR",
               Reputation = 100,
            });
        }

        [HttpPost]
        public IActionResult Post(UserRegistration userRegistration)
        {
            return Ok(new User
            {
                Id = 1,
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,
                Email = userRegistration.Email,
                Reputation = 0,
            });
        }
        
        [HttpPut]
        public IActionResult Put(User user)
        {
            return Ok(user);
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

        [HttpPost("auth")]
        public IActionResult Post(UserAuth authRequest)
        {
            return Ok(new UserAuthResponse
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0b3B0YWwuY29tIiwiZXhw"
            });
        }
    }
}
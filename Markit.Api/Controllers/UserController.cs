using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Get(int id)
        {
            return Ok( new User
            {
               Id = id,
               FirstName = "TestFirstName",
               LastName = "TestLastName",
               Email = "Test@example.com",
               UserName = "TEsTUseR",
               Reputation = 100,
               ReputationLastUpdated = DateTime.Now,
               CreatedAt = DateTime.Now,
               UpdatedAt = DateTime.Now
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
                ReputationLastUpdated = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
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
        public IActionResult GetAll(string userId)
        {
            return Ok($"userId: {userId}");
        }
        
        [HttpGet("{userId}/list/{listId}/analyze")]
        public IActionResult Get(string userId, string listId)
        {
            return Ok($"userId: {userId}, listId: {listId}");
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
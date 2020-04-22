using System;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Markit.Api.Controllers
{
    [ApiController, Route("healthcheck")]
    public class HealthCheckController : Controller
    {
        private readonly IConfiguration _configuration;
        public HealthCheckController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new Healthcheck
            {
                AmIHealthy = true,
                Message = $"Connection String: {_configuration.GetConnectionString("MySql")}, EnvVar: {Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb")}"
            });
        }
    }
}
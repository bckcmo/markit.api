using System;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Markit.Api.Controllers
{
    [ApiController, Route("healthcheck")]
    public class HealthCheckController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseUtil _databaseUtil;
        public HealthCheckController(IConfiguration configuration, IDatabaseUtil databaseUtil)
        {
            _configuration = configuration;
            _databaseUtil = databaseUtil;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new Healthcheck
            {
                AmIHealthy = true,
                Message = $"{Environment.GetEnvironmentVariable("JWT_KEY")} " +
                          $": {Environment.GetEnvironmentVariable("JWT_ISSUER")} " +
                          $": {Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb")}" +
                          $": {_databaseUtil.GetConnectionString()}"
            });
        }
    }
}
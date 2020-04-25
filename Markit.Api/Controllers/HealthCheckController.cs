﻿using System;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
                Message = $"{Environment.GetEnvironmentVariable("JWT_KEY")} : {Environment.GetEnvironmentVariable("JWT_ISSUER")} : {Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb")}"
            });
        }
    }
}
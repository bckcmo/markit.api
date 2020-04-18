using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("healthcheck")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new Healthcheck
            {
                AmIHealthy = true
            });
        }
    }
}
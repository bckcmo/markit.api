using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    [ApiController, Route("healthcheck")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Json(new Dictionary<string, bool> { {"healthy", true} });
        }
    }
}
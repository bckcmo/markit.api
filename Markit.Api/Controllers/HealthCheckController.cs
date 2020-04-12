using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    public class HealthCheckController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
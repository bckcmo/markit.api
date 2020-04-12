using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    public class TagController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    public class ItemController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
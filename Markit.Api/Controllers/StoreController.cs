using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
{
    public class StoreController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
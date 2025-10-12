using Microsoft.AspNetCore.Mvc;

namespace WebDoDungNhaBep.Areas.Admin.ControllerAdmin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

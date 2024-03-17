using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Admin()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

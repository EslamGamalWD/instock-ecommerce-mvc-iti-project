using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Test()
    {
        return View();
    }
}
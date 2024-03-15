using System.Security.Claims;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is not null)
        {
            var userId = claim.Value;
            var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
            HttpContext.Session.SetInt32("shoppingCartSession", count);
        }

        return View();
    }

    public IActionResult Test()
    {
        return View();
    }
}
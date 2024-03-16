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
    public async Task<IActionResult> Checkout()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null)
          return View("NotValidUser");
        //Check if the user didn't add their details before(First time To Order)
        var userId = claim.Value;
        if (await UserDataExist(userId)) return View("PaymentView");
        return View("ChechoutDetails");
    }

    private async Task<bool> UserDataExist(string userId)
    {
        var user= await _unitOfWork.UserRepository.GetUserById(userId);
        if(user?.CityName != null) return true;
         return false;
    }

    public IActionResult Test()
    {
        return View();
    }
}
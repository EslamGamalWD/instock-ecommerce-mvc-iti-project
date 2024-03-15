using System.Security.Claims;
using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers;


public class CartController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CartController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var cart = await _unitOfWork.CartRepository.GetUserCart();
        return View(cart);
    }

    public async Task<IActionResult> Details(int productId)
    {
        if (ModelState.IsValid)
        {
            // var claimsIdentity = (ClaimsIdentity)User.Identity;
            // var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            // var userId = claim.Value;
            var userId = "97375f38-636c-49f9-a8e4-aab67ffd2c16";
            var user = await _unitOfWork.UserRepository.GetUser(userId);
            await _unitOfWork.CartRepository.AddItem(productId, quantity: 1, user);

            var count = await _unitOfWork.CartRepository.GetCartItemsCount();
            
            HttpContext.Session.SetInt32("shoppingCartSession", count);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index", "FilterProduct");
    }

    // public async Task<IActionResult> AddToShoppingCart(int productId, int quantity = 1,
    //     int redirect = 0)
    // {
    //     var cartItemsCount = await _unitOfWork.CartRepository.AddItem(productId, quantity);
    //     if (redirect == 0)
    //         return Ok(cartItemsCount);
    //
    //     return RedirectToAction("Index");
    // }
    //
    // public async Task<IActionResult> RemoveFromShoppingCart(int productId)
    // {
    //     var cartItemsCount = await _unitOfWork.CartRepository.RemoveItem(productId);
    //
    //     return RedirectToAction("Index");
    // }
    //
    // public IActionResult GetTotalItemsCount()
    // {
    //     var count = _unitOfWork.CartRepository.GetCartItemsCount();
    //     return Ok(count);
    // }
}
using System.Security.Claims;
using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.CartVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace InStockWebAppPL.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;

    public CartController(IMapper mapper, IUnitOfWork unitOfWork, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        this.signInManager=signInManager;
        this.userManager=userManager;
    }
    [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]

    public async Task<IActionResult> Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        string userId = string.Empty;

        if (claim is not null)
        {
            userId = claim.Value;
            var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
            HttpContext.Session.SetInt32("shoppingCartSession", count);
        }

        var cart = await _unitOfWork.CartRepository.GetCart(userId);
        ViewBag.Cart  = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
        var cartViewModel = _mapper.Map<CartVM?>(cart);
        return View(cartViewModel);
    }
    [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]

    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        if (ModelState.IsValid)
        {
            var userId = await GetUserId();
            var user = await _unitOfWork.UserRepository.GetUser(userId);
            var cart = await _unitOfWork.CartRepository.GetCart(userId);

            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = user.Id,
                    User = user
                };
                await _unitOfWork.CartRepository.Add(cart);
            }

            var product = await _unitOfWork.ProductRepository.GetById(productId);
            var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(i =>
                i.ProductId == productId &&
                i.CartId == cart.Id &&
                !i.IsSelected &&
                !i.IsDeleted);

            if (item is null)
            {
                if (product?.InStock >= quantity)
                {
                    item = new Item
                    {
                        ProductId =productId,
                        Product = product,
                        Cart = cart,
                        CartId = cart.Id,
                        Quantity = quantity,
                        TotalPrice = quantity * product.Price
                    };
                    await _unitOfWork.ItemRepository.Add(item);
                }
                else
                {
                    return RedirectToAction("Index", "FilterProduct");
                }
            }
            else
            {
                if (product?.InStock >= item.Quantity + quantity)
                {
                    item.Quantity += quantity;
                    item.TotalPrice += quantity * product.Price;
                }
                else
                {
                    return RedirectToAction("Index", "FilterProduct");
                }
            }

            cart.ModifiedAt = DateTime.Now;
            cart.TotalPrice = CalculateCartTotalPrice(cart);
            await _unitOfWork.Save();

            var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
            HttpContext.Session.SetInt32("shoppingCartSession", count);
            return Json(new { success = true, cartCount = count });
        }

        return RedirectToAction("Index", "FilterProduct");
    }

    [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]

    [HttpPost]
    public async Task<IActionResult> IncreaseItemCount(string itemId)
    {
        var userId = await GetUserId();
        var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(i => i.Id == int.Parse(itemId) &&
            !i.IsDeleted, i => i.Product);

        if (item.Quantity < item.Product.InStock)
        {
            item.Quantity += 1;
            item.TotalPrice += item.Product.Price;
            item.ModifiedAt = DateTime.Now;
            await UpdateCartTotalPrice(userId);
            await _unitOfWork.Save();
        }
        var totalcount = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
        var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
        var cart = await _unitOfWork.CartRepository.GetCart(userId);

        return Json(new { success = true, TotalPrice = item.TotalPrice, Quantity = item.Quantity, total = totalcount, instock = product.InStock, cardPrice = cart.TotalPrice });
    }

    [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]

    [HttpPost]
    public async Task<IActionResult> DecreaseItemCount(string itemId)
    {
        var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(
            i => i.Id == int.Parse(itemId) && !i.IsDeleted,
            i => i.Product);

        if (item == null)
        {
            return Json(new { success = false, error = "Item not found" });
        }

        var userId =await GetUserId();
        var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);

        if (item.Quantity == 1)
        {
            item.DeletedAt = DateTime.Now;
            item.ModifiedAt = DateTime.Now;
            item.IsDeleted = true;
        }
        else
        {
            item.Quantity -= 1;
            item.TotalPrice -= item.Product.Price;
        }

        HttpContext.Session.SetInt32("shoppingCartSession", count - 1);
        await UpdateCartTotalPrice(userId);
        await _unitOfWork.Save();
        var totalcount = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
        var cart = await _unitOfWork.CartRepository.GetCart(userId);
        var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);


        return Json(new { success = true, TotalPrice = item.TotalPrice, instock = product.InStock, Quantity = item.Quantity, total = totalcount, IsDeleted = item.IsDeleted, cardPrice = cart.TotalPrice });
    }
    [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteItem(string itemId)
    {
        var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(i => i.Id ==  int.Parse(itemId) &&
            !i.IsDeleted, i => i.Product);
        var userId = await GetUserId();
        var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
        HttpContext.Session.SetInt32("shoppingCartSession", count - item.Quantity);

        item.Quantity = 0;
        item.TotalPrice = 0;
        item.IsDeleted = true;
        item.DeletedAt = DateTime.Now;
        item.ModifiedAt = DateTime.Now;

        await UpdateCartTotalPrice(userId);
        await _unitOfWork.Save();
        var cart = await _unitOfWork.CartRepository.GetCart(userId);
        var totalcount = await _unitOfWork.CartRepository.GetCartItemsCount(userId);

        return Json(new { success = true, cardPrice = cart.TotalPrice, total = totalcount });
    }

    private async Task<string>  GetUserId()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);

        return user.Id;
    }

    private decimal CalculateCartTotalPrice(Cart cart) =>
        cart.Items.Where(i => !i.IsDeleted).Sum(i => i.TotalPrice);

    private async Task UpdateCartTotalPrice(string userId)
    {
        var cart = await _unitOfWork.CartRepository.GetCart(userId);
        cart.TotalPrice = CalculateCartTotalPrice(cart);
    }
}
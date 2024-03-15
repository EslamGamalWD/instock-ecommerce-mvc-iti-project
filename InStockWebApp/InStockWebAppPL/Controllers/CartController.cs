using System.Security.Claims;
using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Models.CartVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers;

[Authorize]
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
        var cartViewModel = _mapper.Map<CartVM>(cart);
        return View(cartViewModel);
    }

    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        if (ModelState.IsValid)
        {
            var userId = GetUserId();
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
            var item = await _unitOfWork.ItemRepository
                .GetFirstOrDefault(i =>
                    i.ProductId == productId &&
                    i.CartId == cart.Id &&
                    !i.IsSelected &&
                    !i.IsDeleted);

            if (item is null)
            {
                if (product.InStock >= quantity)
                {
                    item = new Item
                    {
                        ProductId = productId,
                        Product = product,
                        Cart = cart,
                        CartId = cart.Id,
                        Quantity = quantity,
                        TotalPrice = quantity * product.Price
                    };
                    await _unitOfWork.ItemRepository.Add(item);
                }
                else
                    return RedirectToAction("Index", "FilterProduct");
            }
            else
            {
                quantity = item.Quantity;
                if (product.InStock > quantity)
                {
                    item.Quantity += quantity;
                    item.TotalPrice += quantity * product.Price;
                }
                else
                    return RedirectToAction("Index", "FilterProduct");
            }

            cart.ModifiedAt = DateTime.Now;
            cart.TotalPrice = CalculateCartTotalPrice(cart);
            await _unitOfWork.Save();
            
            var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
            HttpContext.Session.SetInt32("shoppingCartSession", count);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index", "FilterProduct");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IncreaseItemCount(int itemId)
    {
        var userId = GetUserId();
        var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(i => i.Id == itemId &&
            !i.IsDeleted, i => i.Product);

        if (item.Quantity < item.Product.InStock)
        {
            item.Quantity += 1;
            item.TotalPrice += item.Product.Price;
            item.ModifiedAt = DateTime.Now;
            await UpdateCartTotalPrice(userId);
            await _unitOfWork.Save();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DecreaseItemCount(int itemId)
    {
        var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(i => i.Id == itemId &&
            !i.IsDeleted, i => i.Product);
        var userId = GetUserId();
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

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteItem(int itemId)
    {
        var item = await _unitOfWork.ItemRepository.GetFirstOrDefault(i => i.Id == itemId &&
            !i.IsDeleted, i => i.Product);
        var userId = GetUserId();
        var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
        HttpContext.Session.SetInt32("shoppingCartSession", count - item.Quantity);

        item.Quantity = 0;
        item.TotalPrice = 0;
        item.IsDeleted = true;
        item.DeletedAt = DateTime.Now;
        item.ModifiedAt = DateTime.Now;

        await UpdateCartTotalPrice(userId);
        await _unitOfWork.Save();

        return RedirectToAction("Index");
    }

    private string GetUserId()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        return claim.Value;
    }

    private decimal CalculateCartTotalPrice(Cart cart) =>
        cart.Items.Where(i=>!i.IsDeleted).Sum(i => i.TotalPrice);

    private async Task UpdateCartTotalPrice(string userId)
    {
        var cart = await _unitOfWork.CartRepository.GetCart(userId);
        cart.TotalPrice = CalculateCartTotalPrice(cart);
    }
}
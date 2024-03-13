using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
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

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AddToShoppingCart(int productId)
    {
        var userName = User.Identity?.Name;
        var currentUserCart = await _unitOfWork.CartRepository.GetCart(userName);
        var item = await _unitOfWork.ItemRepository
            .GetFirstOrDefault(i => i.ProductId == productId && !i.IsSelected);

        if (item is null)
        {
            item = new Item
            {
                Quantity = 1,
                ProductId = productId
                // Product = product
            };
        }
        else
        {
            item.Quantity++;
        }

        item.CartId = currentUserCart.Id;
        item.Cart = currentUserCart;
        await _unitOfWork.Save();

        return RedirectToAction("Index");
    }
}
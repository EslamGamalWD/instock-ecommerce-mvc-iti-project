using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppDAL.Entities;
using System.Security.Claims;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InStockWebAppBLL.Models.UserVM;
using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;

namespace InStockWebAppPL.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IDiscountRepository _discountRepository;
    private readonly ICheckOutRepository checkOutRepository;
    private readonly IUserRepository userRepository;
    private readonly IGetCategoryWithProductRepository getCategoryWithProductRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ICategoryRepository categoryRepository,
       
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDiscountRepository discountRepository, ICheckOutRepository checkOutRepository,IUserRepository userRepository, IGetCategoryWithProductRepository getCategoryWithProductRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _discountRepository = discountRepository;
        this.checkOutRepository=checkOutRepository;
        this.userRepository=userRepository;
        this.getCategoryWithProductRepository=getCategoryWithProductRepository;
        _unitOfWork = unitOfWork;
	}
    [ResponseCache(Duration = 0,NoStore =true,Location =ResponseCacheLocation.Client)]
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

        var categories = await _categoryRepository.GetAll();

        var allDiscounts = await _discountRepository.GetAll();
        var discounts = allDiscounts.Where(d => d.IsActive).ToList();
        ViewBag.Discounts = discounts;

        var productsWithDiscount = await _productRepository.GetProductsWithActiveDiscount();
        ViewBag.DiscountedProducts = productsWithDiscount;

        var productsOrderedByUnitsSold = await _productRepository.GetProductsOrderedByUnitsSold();
        ViewBag.ProductsByUnitsSold = productsOrderedByUnitsSold;

        return View(await getCategoryWithProductRepository.CategoryWithProducts(categories));
    }

    public async Task<IActionResult> Checkout()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null)
            return View("NotValidUser");
        //Check if the user didn't add their details before(First time To Order)
        var userId = claim.Value;
        if (await userRepository.UserDataExist(userId)) return RedirectToAction("CreatePaymentSession", "Payment", new { userId=userId });
        var user =
            _mapper.Map<UserCheckoutDetailsVM>(
                await _unitOfWork.UserRepository.GetUserById(userId));
        return View("CheckoutDetails", user);
    }

    public async Task<IActionResult> Edit(UserCheckoutDetailsVM modelVM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (await checkOutRepository.CheckoutEdit(modelVM))
                {
                    return RedirectToAction("CreatePaymentSession", "Payment", new { userId = modelVM.Id });
                }
                else
                {
                    //    TempData["Message"] = null;
                    //    TempData["Check"] = "Error There Are Problem";
                    return View("CheckoutDetails", modelVM);
                }
            }
        }
        catch (Exception)
        {
            //TempData["Message"] = null;

            return View("CheckoutDetails", modelVM);
        }

        //TempData["Check"] = "Check You Data inputs ";
        //TempData["Message"] = null;

        return View("CheckoutDetails", modelVM);
    }
}
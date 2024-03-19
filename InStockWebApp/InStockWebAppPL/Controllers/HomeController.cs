using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppDAL.Entities;
﻿using System.Security.Claims;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InStockWebAppBLL.Models.UserVM;
using AutoMapper;

namespace InStockWebAppPL.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

	public HomeController(ICategoryRepository categoryRepository,
                          ISubCategoryRepository subCategoryRepository,
                          IProductRepository productRepository,
						  IUnitOfWork unitOfWork,
                          IMapper mapper,
                          IDiscountRepository discountRepository)
    {
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _productRepository = productRepository;
		_unitOfWork = unitOfWork;
        _mapper = mapper;
        _discountRepository = discountRepository;
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

		var categories = await _categoryRepository.GetAll();
        var categoriesWithProductsVMs = new List<CategoryWithProductsVM>();

        foreach (var category in categories)
        {
            if (category.IsDeleted == true) { continue; }

            var categoryWithProductsVM = new CategoryWithProductsVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                ModifiedAt = category.ModifiedAt,
                DeletedAt = category.DeletedAt,
                IsDeleted = category.IsDeleted,
                SubCategories = await _subCategoryRepository.getAllSubCategoriesByCategoryId(category.Id)
            };

            if (categoryWithProductsVM.SubCategories != null)
            {
                foreach (var subcategory in categoryWithProductsVM.SubCategories)
                {
                    if (subcategory.IsDeleted == true) { continue; }

                    subcategory.Products = await _productRepository.GetProductsBySubcategoryId(subcategory.Id);

                    if (subcategory.Products != null)
                    {
                        categoryWithProductsVM.Products ??= new List<Product>();
                        categoryWithProductsVM.Products.AddRange(subcategory.Products);
                    }
                }
            }

            if (categoryWithProductsVM.Products != null)
            {
                var rnd = new Random();
                categoryWithProductsVM.Products = categoryWithProductsVM.Products.OrderBy(p => rnd.Next()).ToList();
            }

            categoriesWithProductsVMs.Add(categoryWithProductsVM);
        }

        var allDiscounts = await _discountRepository.GetAll();
        var discounts = allDiscounts.Where(d => d.IsActive).ToList();
        ViewBag.Discounts = discounts;

        var productsWithDiscount = await _productRepository.GetProductsWithActiveDiscount();
        ViewBag.DiscountedProducts = productsWithDiscount;

        var productsOrderedByUnitsSold = await _productRepository.GetProductsOrderedByUnitsSold();
        ViewBag.ProductsByUnitsSold = productsOrderedByUnitsSold;

        return View(categoriesWithProductsVMs);
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
        var user = _mapper.Map<UserCheckoutDetailsVM>(await _unitOfWork.UserRepository.GetUserById(userId));
        return View("CheckoutDetails",user);
    }

    public async Task<IActionResult> Edit(UserCheckoutDetailsVM modelVM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (await _unitOfWork.UserRepository.CheckoutEdit(modelVM))
                {
                    return View("PaymentView");

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

    private async Task<bool> UserDataExist(string userId)
    {
        var user= await _unitOfWork.UserRepository.GetUserById(userId);
        if (user != null &&
         !string.IsNullOrEmpty(user.CityName) &&
         !string.IsNullOrEmpty(user.StateName) &&
         !string.IsNullOrEmpty(user.LastName) &&
         !string.IsNullOrEmpty(user.FirstName) &&
         !string.IsNullOrEmpty(user.PhoneNumber) &&
         !string.IsNullOrEmpty(user.Email))
        {
            return true;
        }
         return false;
    }

    public async Task<IActionResult> Test()
    {
        var categories = await _categoryRepository.GetAll();

        var categoriesWithProductsVMs = new List<CategoryWithProductsVM>();

        foreach (var category in categories)
        {
            var categoryWithProductsVM = new CategoryWithProductsVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                ModifiedAt = category.ModifiedAt,
                DeletedAt = category.DeletedAt,
                IsDeleted = category.IsDeleted,
                SubCategories = await _subCategoryRepository.getAllSubCategoriesByCategoryId(category.Id)
            };

            if (categoryWithProductsVM.SubCategories != null)
            {
                foreach (var subcategory in categoryWithProductsVM.SubCategories)
                {
                    subcategory.Products = await _productRepository.GetProductsBySubcategoryId(subcategory.Id);

                    if (subcategory.Products != null)
                    {
                        categoryWithProductsVM.Products ??= new List<Product>();
                        categoryWithProductsVM.Products.AddRange(subcategory.Products);
                    }
                }
            }

            if (categoryWithProductsVM.Products != null)
            {
                var rnd = new Random();
                categoryWithProductsVM.Products = categoryWithProductsVM.Products.OrderBy(p => rnd.Next()).ToList();
            }

            categoriesWithProductsVMs.Add(categoryWithProductsVM);
        }

        return View(categoriesWithProductsVMs);
    }
}
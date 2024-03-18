using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppDAL.Entities;
﻿using System.Security.Claims;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace InStockWebAppPL.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ICategoryRepository categoryRepository,
        ISubCategoryRepository subCategoryRepository,
        IProductRepository productRepository,
        IDiscountRepository discountRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _productRepository = productRepository;
        _discountRepository = discountRepository;
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

        var categories = await _categoryRepository.GetAll();
        var categoriesWithProductsVMs = new List<CategoryWithProductsVM>();

        foreach (var category in categories)
        {
            if (category.IsDeleted == true)
            {
                continue;
            }

            var categoryWithProductsVM = new CategoryWithProductsVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                ModifiedAt = category.ModifiedAt,
                DeletedAt = category.DeletedAt,
                IsDeleted = category.IsDeleted,
                SubCategories =
                    await _subCategoryRepository.getAllSubCategoriesByCategoryId(category.Id)
            };

            if (categoryWithProductsVM.SubCategories != null)
            {
                foreach (var subcategory in categoryWithProductsVM.SubCategories)
                {
                    if (subcategory.IsDeleted == true)
                    {
                        continue;
                    }

                    subcategory.Products =
                        await _productRepository.GetProductsBySubcategoryId(subcategory.Id);

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
                categoryWithProductsVM.Products =
                    categoryWithProductsVM.Products.OrderBy(p => rnd.Next()).ToList();
            }

            categoriesWithProductsVMs.Add(categoryWithProductsVM);
        }

        var allDiscounts = await _discountRepository.GetAll();
        var discounts = allDiscounts.Where(d => d.IsActive).ToList();
        ViewBag.Discounts = discounts;

        var productsWithDiscount = await _productRepository.GetProductsWithActiveDiscount();
        ViewBag.DiscountedProducts = productsWithDiscount;

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
        return View("ChechoutDetails");
    }

    private async Task<bool> UserDataExist(string userId)
    {
        var user = await _unitOfWork.UserRepository.GetUserById(userId);
        if (user?.CityName != null) return true;
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
                SubCategories =
                    await _subCategoryRepository.getAllSubCategoriesByCategoryId(category.Id)
            };

            if (categoryWithProductsVM.SubCategories != null)
            {
                foreach (var subcategory in categoryWithProductsVM.SubCategories)
                {
                    subcategory.Products =
                        await _productRepository.GetProductsBySubcategoryId(subcategory.Id);

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
                categoryWithProductsVM.Products =
                    categoryWithProductsVM.Products.OrderBy(p => rnd.Next()).ToList();
            }

            categoriesWithProductsVMs.Add(categoryWithProductsVM);
        }

        return View(categoriesWithProductsVMs);
    }
}
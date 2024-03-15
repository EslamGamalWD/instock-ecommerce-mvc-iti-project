using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IProductRepository _productRepository;

    public HomeController(ICategoryRepository categoryRepository,
                          ISubCategoryRepository subCategoryRepository,
                          IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _productRepository = productRepository;
    }

    //public async Task<IActionResult> Index()
    //{
    //    var categoriesWithSubcategories = await _categoryRepository.GetAll();

    //    foreach (var category in categoriesWithSubcategories)
    //    {
    //        category.SubCategories = await _subCategoryRepository.getAllSubCategoriesByCategoryId(category.Id);

    //        foreach (var subcategory in category.SubCategories)
    //        {
    //            subcategory.Products = await _productRepository.GetProductsBySubcategoryId(subcategory.Id);
    //        }
    //    }

    //    return View(categoriesWithSubcategories);
    //}

    public async Task<IActionResult> Index()
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
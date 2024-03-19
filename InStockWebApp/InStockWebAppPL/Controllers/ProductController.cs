
﻿using InStockWebAppBLL.Features.Interfaces.Domain;
﻿
//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InStockWebAppPL.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductImageRepository _imageRepository;


        //private readonly Cloudinary _cloudinary;

        

        public ProductController(IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,IProductImageRepository imageRepository,IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _imageRepository=imageRepository;

            _discountRepository = discountRepository;

            //Account account = new()
            //{
            //    Cloud=cloudinary.Value.Cloud,
            //    ApiKey = cloudinary.Value.ApiKey,
            //    ApiSecret = cloudinary.Value.ApiSecret
            //};
            //_cloudinary = new Cloudinary(account);

        }

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetAll());
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _productRepository.Details(id));
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> CustomerSideDetails(int id)
        {
            var details = await _productRepository.Details(id);

            var product = await _productRepository.GetProductWithSubcategoryById(id);

            var categoryWithProductsVM = new CategoryWithProductsVM
            {
                Id = product.SubCategory.CategoryId,
                Name = product.SubCategory.Name,
                Description = product.SubCategory.Description,
                CreatedAt = product.SubCategory.CreatedAt,
                ModifiedAt = product.SubCategory.ModifiedAt,
                DeletedAt = product.SubCategory.DeletedAt,
                IsDeleted = product.SubCategory.IsDeleted,
                SubCategories = await _subCategoryRepository.getAllSubCategoriesByCategoryId(product.SubCategory.CategoryId)
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
                categoryWithProductsVM.Products = categoryWithProductsVM.Products.Where(p => p.Id != id).ToList();
            }

            if (categoryWithProductsVM.Products != null)
            {
                var rnd = new Random();
                categoryWithProductsVM.Products = categoryWithProductsVM.Products.OrderBy(p => rnd.Next()).ToList();
            }

            ViewBag.RelatedProducts = categoryWithProductsVM.Products;

            return View(details);
        }

        [HttpGet]
        // GET: ProductController/Create
        public async Task<IActionResult> Create()
        {
            ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
            ViewData["DiscountId"] = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        public async Task<IActionResult> Create(AlterProductVM entityVM)
        {
            try
            {
                ModelState["ImageFiles"].ValidationState = ModelValidationState.Valid;
                if (ModelState.IsValid)
                {
                   
                    int createdId=await _productRepository.Add(entityVM);
                    if (createdId != -1)
                    {
                        await _imageRepository.add(entityVM.ImageFiles,createdId);
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
                ViewData["DiscountId"] = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
                return View(entityVM);
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {   if (id == null) return NotFound();
            var product = await _productRepository.EditDetails(id);
            if(product==null)return NotFound();
            ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
            ViewData["DiscountId"] = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AlterProductVM entityVM)
        {
            try
            {
                ModelState["ImageFiles"].ValidationState= ModelValidationState.Valid;
                if (ModelState.IsValid) { 
                    await _productRepository.Update(id,entityVM);
                    await _imageRepository.add(entityVM.ImageFiles, id);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
                ViewData["DiscountId"] = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
                return View(entityVM);
            }
            catch
            {
                return View();
            }
        }
        //Post: ProductController/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int? id)
        {
            var toggleDateTime = await _productRepository.ToggleStatus(id);
            if (toggleDateTime !=null)
                return Ok(toggleDateTime);
            return NotFound();
        }
    }
}

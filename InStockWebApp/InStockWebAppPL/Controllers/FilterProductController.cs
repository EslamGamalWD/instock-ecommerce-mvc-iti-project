using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.OutputCaching;

namespace InStockWebAppPL.Controllers
{
    public class FilterProductController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilterRepository filter;
        private readonly ISubCategoryRepository subCategoryRepository;

        public FilterProductController(IFilterRepository filter,
            ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            _unitOfWork = unitOfWork;
            this.filter = filter;
            this.subCategoryRepository = subCategoryRepository;
        }

        [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]

        public async Task<IActionResult> Index(int ? id = null)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is not null)
            {
                var userId = claim.Value;
                var count = await _unitOfWork.CartRepository.GetCartItemsCount(userId);
                HttpContext.Session.SetInt32("shoppingCartSession", count);
            }

            var pageSize = 6;


            ViewBag.Catogery = mapper
                .Map<IEnumerable<GetAllCategoriesVM>>
                    (await categoryRepository.GetAll());

            ViewBag.SubCategory = mapper
                .Map<IEnumerable<SubcategoryVM>>
                    (await subCategoryRepository.GetAll());



            ViewBag.TotalPages = await filter.totalCount();

            ViewBag.CurrentPage = (int)Math.Ceiling(await filter.totalCount() / (double)pageSize);
            TempData["CategoeyID"]=id;
            var products = await filter.GetProductsForPage(1, pageSize);
            var totalcount = await filter.totalCount();
            ViewBag.TotalPages = totalcount;

            ViewBag.CurrentPage = 1;

            ViewBag.ProductCount = totalcount;
            var result = products.Where(a => a.SubCategory.CategoryId ==id).ToList();
            return View(result);
        }
        [ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.Client)]


        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 6, string sortOption = "default", int? categoryId = null, string subcategoryIds = null, int minPrice = 0, int maxPrice = 1000000, string search = null)
        {
            int? categoryid = (int?)TempData["CategoeyID"];
            if(categoryid !=null)
            {
                categoryId = categoryid;

            }
            var productList = await filter.GetByFilter(page, pageSize, sortOption, categoryId, subcategoryIds, minPrice, maxPrice, search);

            var totalProducts = await filter.totalCount();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            return PartialView("_ProductList", productList);
        }

        public IActionResult Sort(string sortOption)
        {
            return RedirectToAction("Get", new { sortOption = sortOption });
        }

        public async Task<IActionResult> GetSubcategories(int categoryId)
        {
            var subCategory = mapper.Map<IEnumerable<SubcategoryVM>>(
                await subCategoryRepository.getAllSubCategoriesByCategoryId(categoryId));
            return PartialView("_SubCategoryFilter", subCategory);
        }
    }
} 
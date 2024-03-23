using InStockWebAppBLL.Features.Interfaces.Domain;

using InStockWebAppBLL.Models.HomeVM;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppPL.Resources;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using InStockWebAppBLL.Helpers.Role;

namespace InStockWebAppPL.Controllers
{
    [Authorize(Roles = @$"{AppRoles.Admin}")]

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductImageRepository _imageRepository;
        




        public ProductController(IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,IProductImageRepository imageRepository,
            IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _imageRepository=imageRepository;
            _discountRepository = discountRepository;

        }
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetAll());
        }
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _productRepository.Details(id));
        }


        [AllowAnonymous]

        public async Task<IActionResult> ProductDetailes(int? id)
        {
            var details = await _productRepository.Details(id);

            return PartialView("_ProductDetailes", details);
        }


        [AllowAnonymous]
        // GET: ProductController/Details/5
        public async Task<IActionResult> CustomerSideDetails(int id)
        {
            ViewBag.ID = id;
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
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        [HttpGet]
        // GET: ProductController/Create
        public async Task<IActionResult> Create()
        {
            ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
            var emptyOption = new SelectListItem() { Value = "", Text = "No Discount" };
            var selectList = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
            var finalList = new List<SelectListItem> { emptyOption };
            finalList.AddRange(selectList);
            ViewData["DiscountId"] = finalList;
            return View();
        }
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        // POST: ProductController/Create
        [HttpPost]
        public async Task<IActionResult> Create(AlterProductVM entityVM, List<IFormFile>? ImageFiles)
        {
            try
            {
                ModelState["ImageFiles"].ValidationState = ModelValidationState.Valid;
                if (ModelState.IsValid)
                {
                   
                    int createdId=await _productRepository.Add(entityVM);
                    if (createdId != -1)
                    {
                        await _imageRepository.add(ImageFiles,createdId);
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
                var emptyOption = new SelectListItem() { Value = "", Text = "No Discount" };
                var selectList = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
                var finalList = new List<SelectListItem> { emptyOption };
                finalList.AddRange(selectList);
                ViewData["DiscountId"] = finalList;
                return View(entityVM);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {   if (id == null) return NotFound();
            var product = await _productRepository.EditDetails(id);
            if(product==null)return NotFound();
            ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
            var emptyOption = new SelectListItem() { Value = "", Text = "No Discount" };
            var selectList = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
            var finalList = new List<SelectListItem> { emptyOption };
            finalList.AddRange(selectList);
            ViewData["DiscountId"] = finalList;
            return View(product);
        }
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        // POST: ProductController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AlterProductVM entityVM,List<IFormFile>? ImageFiles)
        {
            try
            {
                ModelState["ImageFiles"].ValidationState= ModelValidationState.Valid;
                if (ModelState.IsValid) { 
                    var oldProduct=await _productRepository.GetById(id);
                    await _productRepository.Update(id,entityVM);
                    //Delete Images if no other product is referring to it.
                    await _imageRepository.remove(oldProduct.Images, id);
                    await _imageRepository.add(ImageFiles, id);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
                var emptyOption = new SelectListItem() { Value = "", Text = "No Discount" };
                var selectList = new SelectList(await _discountRepository.GetAll(), "Id", "Name");
                var finalList = new List<SelectListItem>{ emptyOption};
                finalList.AddRange(selectList);
                ViewData["DiscountId"] = finalList;
                return View(entityVM);
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = @$"{AppRoles.Admin}")]

        //Post: ProductController/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var toggleDateTime = await _productRepository.ToggleStatus(id);
            if (toggleDateTime !=null)
                return Ok(toggleDateTime);
            return NotFound();
        }
    }
}

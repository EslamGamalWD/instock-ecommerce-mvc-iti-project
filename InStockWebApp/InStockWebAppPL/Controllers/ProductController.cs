
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ProductVM;
using InStockWebAppDAL.Entities;
using InStockWebAppPL.Settings;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace InStockWebAppPL.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        //private readonly IDiscountRepository _discountRepository;
        private readonly IProductImageRepository _imageRepository;
        

        public ProductController(IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,IProductImageRepository imageRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _imageRepository=imageRepository;
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
        [HttpGet]
        // GET: ProductController/Create
        public async Task<IActionResult> Create()
        {
            ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
            var discounts = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Discount 1" },
                    new SelectListItem { Value = "2", Text = "Discount 2" }
                };

            // Create a SelectList for the static list of discounts
            ViewData["DiscountId"] = new SelectList(discounts, "Value", "Text");
            //ViewData["DiscountId"] = new SelectList(await _productRepository.GetAll(), "Id", "Name");
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
                var discounts = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Discount 1" },
                    new SelectListItem { Value = "2", Text = "Discount 2" }
                };

                // Create a SelectList for the static list of discounts
                ViewData["DiscountId"] = new SelectList(discounts, "Value", "Text");
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
            var discounts = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Discount 1" },
                    new SelectListItem { Value = "2", Text = "Discount 2" }
                };

            // Create a SelectList for the static list of discounts
            ViewData["DiscountId"] = new SelectList(discounts, "Value", "Text");
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
                var discounts = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Discount 1" },
                    new SelectListItem { Value = "2", Text = "Discount 2" }
                };

                // Create a SelectList for the static list of discounts
                ViewData["DiscountId"] = new SelectList(discounts, "Value", "Text");
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


using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ProductVM;
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
        

        public ProductController(IProductRepository productRepository,
            ISubCategoryRepository subCategoryRepository,IProductImageRepository imageRepository,IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _imageRepository=imageRepository;
            _discountRepository = discountRepository;
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
            var details=await _productRepository.Details(id);
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

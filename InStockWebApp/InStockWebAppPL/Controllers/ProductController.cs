using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.ProductVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InStockWebAppPL.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        //private readonly IDiscountRepository _discountRepository;

        public ProductController(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            return View(await _productRepository.GetAll());
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            return View(await _productRepository.Details(id));
        }

        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
            ViewData["DiscountId"] = new SelectList(await _productRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AlterProductVM entityVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _productRepository.Add(entityVM);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["SubCategoryId"] = new SelectList(await _subCategoryRepository.GetAll(), "Id", "Name");
                ViewData["DiscountId"] = new SelectList(await _productRepository.GetAll(), "Id", "Name");
                return View(entityVM);
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {   if (id == null) return NotFound();
            var product = _productRepository.EditDetails(id);
            if(product==null)return NotFound();
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AlterProductVM entityVM)
        {
            try
            {
                if (ModelState.IsValid) { 
                    await _productRepository.Update(id,entityVM);
                    return RedirectToAction(nameof(Index));
                }
                return View(entityVM);
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

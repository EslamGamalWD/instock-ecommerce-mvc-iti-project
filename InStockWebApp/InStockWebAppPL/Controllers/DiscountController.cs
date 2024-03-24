using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppDAL.Entities;
using InStockWebAppBLL.Models.ProductVM;
using Microsoft.EntityFrameworkCore;
using InStockWebAppBLL.Helpers.ImageUploader;
using InStockWebAppBLL.Helpers.Role;
using Microsoft.AspNetCore.Authorization;

namespace InStockWebAppPL.Controllers
{
   // [Authorize(Roles = @$"{AppRoles.Admin}")]
    public class DiscountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;

        public DiscountController(IMapper mapper, IUnitOfWork unitOfWork,
            IDiscountRepository discountRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var discounts = await _discountRepository.GetAllDiscounts();
            if (discounts is not null)
            {
                return View(discounts);
            }
            else
            {
                ViewBag.Message = "No discounts available at this time.";
                return View(new List<GetAllDiscountsVM>());
            }
        }


        public async Task<IActionResult> Details(int id)
        {
            var selectedDiscount = await _discountRepository.GetDiscountById(id);
            return View(selectedDiscount);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDiscountVM createdDiscount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (createdDiscount.Image != null)
                    {
                        string discountImage =
                            FilesUploader.UploadFile("DiscountImages", createdDiscount.Image);
                        createdDiscount.ImagePath = discountImage;
                    }
                    else
                    {
                        createdDiscount.ImagePath = "noDiscountImage.jpg";
                    }

                    var discount = _mapper.Map<Discount>(createdDiscount);

                    await _discountRepository.Add(discount);

                    await _unitOfWork.Save();

                    TempData["Message"] = "Discount Added Successfully!";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return PartialView("_Create", createdDiscount);
            }

            TempData["Message"] = null;

            return PartialView("_Create", createdDiscount);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var discountEntity = await _discountRepository.GetById(id);
            if (discountEntity == null)
            {
                TempData["Message"] = "Error: Couldn't find a matching discount";
                return RedirectToAction("Index");
            }


            var selectedDiscount = _mapper.Map<UpdateDiscountVM>(discountEntity);


            return View("Edit", selectedDiscount);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateDiscountVM updatedDiscount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var discount = _mapper.Map<Discount>(updatedDiscount);
                    _discountRepository.Update(discount);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["IsValid"] = "true";
                    return View("Edit", updatedDiscount);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: Failed to update discount";
                return RedirectToAction("Edit", updatedDiscount);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var toggleDateTime = await _discountRepository.ToggleStatus(id);
            if (toggleDateTime is { })
                return Ok(toggleDateTime);
            return NotFound();
        }


        public async Task<IActionResult> AssignedProducts(int id)
        {
            var products = await _productRepository.GetAll();
            ViewBag.discountId = id;
            return View("AssignedProducts", products.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDiscountList(int productId, int? discountId,
            bool isInDiscount)
        {
            int? updatedDiscountId = isInDiscount ? discountId : null;
            bool updateSuccess =
                await _discountRepository.ToggleDiscountAssociation(productId, updatedDiscountId);

            if (updateSuccess)
            {
                return Json(new { message = "Update successful." });
            }
            else
            {
                return Json(new { message = "Update failed." });
            }
        }
    }
}
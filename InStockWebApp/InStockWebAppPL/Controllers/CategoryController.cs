using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IMapper mapper, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAll();
            var categoryViewModels = _mapper.Map<IEnumerable<GetAllCategoriesVM>>(categories);

            return View(categoryViewModels);
        }

        //[HttpGet]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var category = await _categoryRepository.GetById(id);

        //    if (category == null)
        //    {
        //        TempData["Message"] = "Error: Category Not Found!";

        //        return RedirectToAction("Index", "Category");
        //    }

        //    TempData["Message"] = null;

        //    return View(category);
        //}

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var categoryDetails = await _categoryRepository.GetCategoryDetailsWithSubCategories(id);

            if (categoryDetails == null)
            {
                TempData["Message"] = "Error: Category Not Found!";
                return RedirectToAction("Index", "Category");
            }

            TempData["Message"] = null;

            return View(categoryDetails);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TempData["Message"] = null;

            return PartialView("_Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(model);

                    if (await _categoryRepository.Add(category))
                    {
                        await _unitOfWork.Save();

                        TempData["Message"] = "Category Added Successfully!";

                        return RedirectToAction("Index", "Category");
                    }
                    else
                    {
                        TempData["Message"] = null;
                        model.Message = "Error: Category Already Exists!";

                        return PartialView("_Create", model);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;
                model.Message = "Error: Enter Your Data Again!";

                return PartialView("_Create", model);
            }

            TempData["Message"] = null;

            return PartialView("_Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category == null)
            {
                TempData["Message"] = "Error: Category Not Found!";

                return RedirectToAction("Index", "Category");
            }

            var categoryViewModel = _mapper.Map<EditCategoryVM>(category);

            TempData["Message"] = null;

            return PartialView("_Edit", categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(model);

                    var existingCategory = await _categoryRepository.GetFirstOrDefault(c => c.Name == category.Name && c.Id != category.Id);

                    if (existingCategory != null)
                    {
                        TempData["Message"] = "Error: Another category with the same name already exists!";

                        return PartialView("_Edit", model);
                    }

                    _categoryRepository.Update(category);
                    await _unitOfWork.Save();

                    TempData["Message"] = "Category Updated Successfully!";

                    return RedirectToAction("Index", "Category");
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "Error: Failed To Update Category!";
            }

            return PartialView("_Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var toggleDateTime = await _categoryRepository.ToggleStatus(id);
            
            if (toggleDateTime is { })
                return Ok(toggleDateTime);
            
            return NotFound();
        }
    }
}

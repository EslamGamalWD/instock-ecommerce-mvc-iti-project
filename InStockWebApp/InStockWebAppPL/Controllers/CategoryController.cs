using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    [Authorize(Roles = @$"{AppRoles.Admin}")]

    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(IMapper mapper, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAll();
            var categoryViewModels = _mapper.Map<IEnumerable<GetAllCategoriesVM>>(categories);

            return View(categoryViewModels);
        }

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

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM model, IFormFile? imageFormFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["message"] =
                        $"{model.Name} Category Added Successfully!";

                    var category = _mapper.Map<Category>(model);

                    if (imageFormFile != null)
                    {
                        string imgExtension = Path.GetExtension(imageFormFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgPath = "\\Files\\CategoryImages\\" + imgName;
                        category.ImagePath = imgPath;

                        string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;

                        FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create);
                        imageFormFile.CopyTo(imgFileStream);
                        imgFileStream.Dispose();
                    }
                    else
                    {
                        category.ImagePath = "\\Files\\CategoryImages\\No_Image.jpg";
                    }

                    if (await _categoryRepository.Add(category))
                    {
                        await _unitOfWork.Save();

                        return RedirectToAction("Index", "Category");
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;
                model.Message = "Error: Enter Your Data Again!";

                return View(model);
            }

            TempData["Message"] = null;

            return View(model);
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

            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryVM model, IFormFile? imageFormFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(model);

                    var oldCategory = await _categoryRepository.GetById(model.ID);

                    var existingCategory = await _categoryRepository.GetFirstOrDefault(c => c.Name == category.Name && c.Id != category.Id);

                    if (existingCategory != null)
                    {
                        TempData["Message"] = "Error: Another category with the same name already exists!";

                        return PartialView("_Edit", model);
                    }

                    if (imageFormFile != null)
                    {
                        if (oldCategory?.ImagePath != "\\Files\\CategoryImages\\No_Image.jpg")
                        {
                            string oldImgFullPath = _webHostEnvironment.WebRootPath + oldCategory.ImagePath;

                            if (System.IO.File.Exists(oldImgFullPath))
                            {
                                System.IO.File.Delete(oldImgFullPath);
                            }
                        }

                        string imgExtension = Path.GetExtension(imageFormFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgPath = "\\Files\\CategoryImages\\" + imgName;
                        category.ImagePath = imgPath;

                        string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;

                        FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create);
                        imageFormFile.CopyTo(imgFileStream);
                        imgFileStream.Dispose();
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

                return View(model);
            }

            return View(model);
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

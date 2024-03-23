using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.CategoryVM;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InStockWebAppPL.Controllers;
[Authorize(Roles = @$"{AppRoles.Admin}")]

public class SubcategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubcategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var subCategories = await _unitOfWork.SubcategoryRepository.GetAll(null,
            s => s.OrderBy(c => c.IsDeleted), s => s.Category);
        var subCategoryVms = _mapper.Map<IEnumerable<SubcategoryVM>>(subCategories);
        return View(subCategoryVms);
    }

    public async Task<IActionResult> Details(int id)
    {
        var subCategoryVm = await GetSubcategoryVmFromSubcategoryById(id);
        return View(subCategoryVm);
    }

    public async Task<IActionResult> Create()
    {
        var categoriesVms = await GetAllCategoryVmsFromCategories();
        ViewBag.CategoryNames = new SelectList(categoriesVms, "Id", "Name", "CategoryId");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SubcategoryVM subcategoryVm)
    {
        try
        {
            if (ModelState.IsValid)
            {
                TempData["message"] =
                    $"{subcategoryVm.Name} subcategory has been created successfully";
                var subcategory = _mapper.Map<SubCategory>(subcategoryVm);
                await _unitOfWork.SubcategoryRepository.Add(subcategory);
                await _unitOfWork.Save();
                return RedirectToAction("Index");
            }
        }
        catch (Exception e)
        {
            // ignored
        }

        return View(subcategoryVm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var categoriesVms = await GetAllCategoryVmsFromCategories();
        ViewBag.CategoryNames = new SelectList(categoriesVms, "Id", "Name", "CategoryId");

        var subcategoryVm = await GetSubcategoryVmFromSubcategoryById(id);
        return View(subcategoryVm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SubcategoryVM subcategoryVm)
    {
        try
        {
            if (ModelState.IsValid)
            {
                TempData["message"] =
                    $"Subcategory#{subcategoryVm.Id} has been updated successfully";
                var subcategory = _mapper.Map<SubCategory>(subcategoryVm);
                _unitOfWork.SubcategoryRepository.Update(subcategory);
                await _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
        }
        catch (Exception e)
        {
            // ignored
        }

        return View(subcategoryVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var subcategory = await _unitOfWork.SubcategoryRepository.GetById(id);
        if (subcategory is null)
            return NotFound();

        subcategory.IsDeleted = !subcategory.IsDeleted;
        if (subcategory.IsDeleted)
        {
            subcategory.DeletedAt = DateTime.Now;
            subcategory.ModifiedAt = DateTime.Now;
        }
        else
        {
            subcategory.DeletedAt = null;
            subcategory.ModifiedAt = DateTime.Now;
        }

        await _unitOfWork.Save();
        return Ok(subcategory.ModifiedAt.ToString());
    }

    private async Task<SubcategoryVM> GetSubcategoryVmFromSubcategoryById(int id)
    {
        var subcategory = await _unitOfWork.SubcategoryRepository.GetFirstOrDefault(s => s.Id == id,
            s => s.Category);
        return _mapper.Map<SubcategoryVM>(subcategory);
    }

    private async Task<IEnumerable<GetAllCategoriesVM>> GetAllCategoryVmsFromCategories()
    {
        var categories = await _unitOfWork.CategoryRepository.GetAll();
        return _mapper.Map<IEnumerable<GetAllCategoriesVM>>(categories);
    }
}
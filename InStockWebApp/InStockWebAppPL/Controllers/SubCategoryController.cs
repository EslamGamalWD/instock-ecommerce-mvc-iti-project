using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppDAL.Context;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InStockWebAppPL.Controllers;

public class SubCategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var subCategories = await _unitOfWork.SubCategoryRepository.GetAll(s => !s.IsDeleted,
            null, s => s.Category);
        var subCategoryVms = _mapper.Map<IEnumerable<SubCategoryVM>>(subCategories);
        return View(subCategoryVms);
    }

    public async Task<IActionResult> Details(int id)
    {
        var subcategory = await _unitOfWork.SubCategoryRepository.GetFirstOrDefault(s => s.Id == id,
            s => s.Category);
        var subCategoryVm = _mapper.Map<SubCategoryVM>(subcategory);
        return View(subCategoryVm);
    }

    public IActionResult Create()
    {
        // ViewBag.CategoryNames = new SelectList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SubCategoryVM subCategoryVm)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var subcategory = _mapper.Map<SubCategory>(subCategoryVm);
                await _unitOfWork.SubCategoryRepository.Add(subcategory);
                await _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // ignored
        }

        return View(subCategoryVm);
    }

    private async Task<SubCategoryVM> GetSubcategoryVmFromSubcategoryById(int id)
    {
        throw new NotImplementedException();
    }
}
using AutoMapper;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Models.SubCategoryVM;
using InStockWebAppDAL.Context;
using Microsoft.AspNetCore.Mvc;

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
        var subCategories = await _unitOfWork.SubCategoryRepository.GetAll(s => !s.IsDeleted);
        var subCategoryVms = _mapper.Map<IEnumerable<SubCategoryVM>>(subCategories);
        return View(subCategoryVms);
    }
}
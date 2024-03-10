using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.RoleVM;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class RoleController : Controller
    {
        #region Prop
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region ctor
        public RoleController(IRoleRepository roleRepository)
        {
            this._roleRepository=roleRepository;
        }
        #endregion


        #region method

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _roleRepository.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TempData["Message"] = null;

            return PartialView("_Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _roleRepository.Create(model))
                    {
                        TempData["Message"] = "saved Successfully";
                        return RedirectToAction("Index", "Role");

                    }
                    else
                    {
                        TempData["Message"] = null;

                        return PartialView("_Create",model);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;

                return PartialView("_Create",model);
            }
            TempData["Message"] = null;

            return PartialView("_Create", model);
        }
        #endregion



    }
}

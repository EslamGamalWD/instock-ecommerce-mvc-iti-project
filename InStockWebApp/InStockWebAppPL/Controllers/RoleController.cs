using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.RoleVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    // [Authorize(Roles = @$"{AppRoles.Admin}")]

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

                        return PartialView("_Create", model);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;

                return PartialView("_Create", model);
            }
            TempData["Message"] = null;

            return PartialView("_Create", model);
        }


        [HttpGet]
        public async Task<IActionResult> Display(string id)
        {

            return View(await _roleRepository.GetUsersRole(id));
        }


        [HttpPost]
        public async Task<IActionResult> Display(CreateUserRolesVM createUserRolesVM)
        {
            if (await _roleRepository.CreateUsersRole(createUserRolesVM))
                return RedirectToAction("Index", "Role");

            TempData["Check"] = "Check Your Data Inputs";
            return View(createUserRolesVM);
        }
        #endregion



    }
}

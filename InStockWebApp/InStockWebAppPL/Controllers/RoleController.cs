using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Models.RoleVM;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class RoleController : Controller
    {
        #region Prop
        private readonly IRoleRepo roleRepo;

        #endregion

        #region ctor
        public RoleController(IRoleRepo roleRepo)
        {
            this.roleRepo=roleRepo;
        }
        #endregion


        #region method

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await roleRepo.GetAll());
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
                    if (await roleRepo.Create(model))
                    {
                        TempData["Message"] = "saved Successfuly";
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

using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Models.UserVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InStockWebAppPL.Controllers
{
    public class UserController : Controller
    {

        #region Prop
        private readonly IUserRepository userRepo;

        #endregion

        #region Ctor
        public UserController(IUserRepository userRepo)
        {
            this.userRepo=userRepo;
        }
        #endregion

        #region Method
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await userRepo.getAll());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM modelVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (modelVM.image != null)
                    {
                       
                        using (var memoryStream = new MemoryStream())
                        {
                            await modelVM.image.CopyToAsync(memoryStream);
                            modelVM.Photo = memoryStream.ToArray();
                        }
                    }

                    if (await userRepo.Create(modelVM))
                    {
                        TempData["Message"] = "saved Successfuly";
                        return RedirectToAction("Index", "User");

                    }
                    else
                    {
                        TempData["Message"] = null;
                        TempData["Check"]="Error There Are The same User Name Or Gmail Try Again";
                        return View("Create", modelVM);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;

                return View("Create", modelVM);
            }
          
            TempData["Check"] = "Check You Data inputs ";
            TempData["Message"] = null;

            return View("Create", modelVM);
        }


        [HttpGet]
        public async Task<IActionResult> Select(string id)
        {
            var user =await userRepo.GetUserById(id);
            return View(user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> ToggleStatus(string id)
        {
            var toggleDateTime = await userRepo.ToggleStatus(id);
           if (toggleDateTime is { })
            return Ok(toggleDateTime);
            return NotFound();
        }
        #endregion

    }
}

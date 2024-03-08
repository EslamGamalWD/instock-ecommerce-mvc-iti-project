﻿using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Repositories;
using InStockWebAppBLL.Models.UserVM;
using Microsoft.AspNetCore.Mvc;

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
                    if (await userRepo.Create(modelVM))
                    {
                        TempData["Message"] = "saved Successfuly";
                        return RedirectToAction("Index", "User");

                    }
                    else
                    {
                        TempData["Message"] = null;

                        return View("Create", modelVM);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;

                return View("Create", modelVM);
            }
            TempData["Message"] = null;

            return View("Create", modelVM);
        }


        [HttpGet]
        public async Task<IActionResult> Select(int id)
        {
            return View("Create");
        }
        #endregion

    }
}

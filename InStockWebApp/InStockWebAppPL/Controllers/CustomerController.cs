using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppBLL.Helpers.ImageUploader;
using InStockWebAppBLL.Helpers.Role;
using Microsoft.AspNetCore.Authorization;

namespace InStockWebAppPL.Controllers
{
    [Authorize]

    public class CustomerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public CustomerController(IMapper mapper, IUnitOfWork unitOfWork, IUserRepository customerRepository ,UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = customerRepository;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var userData = await _userManager.GetUserAsync(HttpContext.User);

            var LoggedUser = await _userRepository.GetUserById(userData.Id);
          
            if (LoggedUser != null)
            {
                var userVM = _mapper.Map<GetUserByIdVM>(LoggedUser);
               
           
                return View("Details", userVM);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit (string id)
        {
            var userData = await _userManager.GetUserAsync(HttpContext.User);

            var LoggedUser = await _userRepository.GetUserById(userData.Id);

           
                var userVM = _mapper.Map<EditUserVM>(LoggedUser);

                return View( userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVM modelVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region Image
                    if (modelVM.Image != null)
                    {
                        string photo = FilesUploader.UploadFile("ImageProfile", modelVM.Image);
                        if (photo== null)
                            modelVM.Photo= "Men.jpg";
                        else
                            modelVM.Photo= photo;


                    }
                    #endregion

                    if (await _userRepository.Edit(modelVM))
                    {
                        TempData["Message"] = "Customer data has been edited successfully";
                        return RedirectToAction("Details", "Customer");

                    }
                    else
                    {
                        TempData["Message"] = null;
                        TempData["Check"] = "There is a problem editing customer details";
                        return View("Edit", modelVM);
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = null;

                return View("Edit", modelVM);
            }

            TempData["Check"] = "Check You Data inputs ";
            TempData["Message"] = null;

            return View("Edit", modelVM);
        }
    }
}

using AutoMapper;
using Hangfire;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.ImageUploader;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
namespace InStockWebAppPL.Controllers
{
    // [Authorize(Roles = @$"{AppRoles.Admin}")]

    public class UserController : Controller
    {

        #region Prop
        private readonly IUserRepository userRepo;
        private readonly IUserPaymentRepository userPaymentRepository;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironmen;
        #endregion
          
        #region Ctor
        public UserController(IUserRepository userRepo, IUserPaymentRepository userPaymentRepository, IMapper mapper, IEmailSender emailSender, IWebHostEnvironment webHostEnvironmen)
        {
            this.userRepo=userRepo;
            this.userPaymentRepository=userPaymentRepository;
            this.mapper=mapper;
            this.emailSender=emailSender;
            this.webHostEnvironmen = webHostEnvironmen;
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
                    #region Image
                    if (modelVM.image != null)
                    {
                        string photo = FilesUploader.UploadFile("ImageProfile", modelVM.image);
                        modelVM.Photo= photo;


                    }
                    else
                    {
                        modelVM.Photo= "Men.jpg";
                    }
                    #endregion

                    var Result = await userRepo.Create(modelVM); 
                    if (Result!=null)
                    {
                        #region UserPayment
                        if (modelVM.UserType==UserType.Customer)
                            await userPaymentRepository.AddListPayment(modelVM.UserPaymentVM, Result);

                        #endregion

                        #region send Email
                        TempData["Message"] = "saved Successfuly";
                        var filePath = $"{webHostEnvironmen.WebRootPath}/Account/Tempelet/Email.html";
                        StreamReader str = new StreamReader(filePath);
                        var body = str.ReadToEnd();
                        str.Close();
                        body = body.Replace("[Header]", "Welcom In Instock Shopping")
                            .Replace("[Body]", "Welcome to InStock! Start managing your inventory efficiently")
                            .Replace("[URL]", "https://localhost:44305/")
                            .Replace("[AncorTitle]", "Go");
                        //Use HangFire
                        BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(modelVM.Email, "Welcome Login", body));
                        
                        #endregion

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
            var user = await userRepo.GetUserById(id);
            return View(user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var toggleDateTime = await userRepo.ToggleStatus(id);

            if (toggleDateTime is { })
                return Ok(toggleDateTime);
            return NotFound();
        }




        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = mapper.Map<EditUserVM>(await userRepo.GetUserById(id));
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVM modelVM)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    if (await userRepo.Edit(modelVM))
                    {
                        TempData["Message"] = "Edit Successfuly";
                        return RedirectToAction("Index", "User");

                    }
                    else
                    {
                        TempData["Message"] = null;
                        TempData["Check"]="Error There Are Problem";
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

        #endregion

    }
}

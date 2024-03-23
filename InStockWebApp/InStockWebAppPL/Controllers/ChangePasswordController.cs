using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using InStockWebAppBLL.Models.ChangePasswordVM;
using InStockWebAppBLL.Helpers.Role;
using Microsoft.AspNetCore.Authorization;

namespace InStockWebAppPL.Controllers
{
    [Authorize]

    public class ChangePasswordController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironmen;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public ChangePasswordController(IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironmen, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.emailSender = emailSender;
            this.webHostEnvironmen = webHostEnvironmen;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                Random random = new Random();

                int randomNumberInRange = random.Next(1, 10000000);
                HttpContext.Session.SetInt32("ChangePassword", randomNumberInRange);


                TempData["Message"] = "Password saved successfully";
                var filePath = $"{webHostEnvironmen.WebRootPath}/Account/Tempelet/Email.html";
                StreamReader str = new StreamReader(filePath);
                var body = str.ReadToEnd();
                str.Close();
                body = body.Replace("[Header]", "Welcome to IN-STOCK")
                    .Replace("[Body]",
                        $"Welcome to InStock!We have received a request to change your password \n {randomNumberInRange}")
                    .Replace("[URL]", "/ChangePassword")
                    .Replace("[AncorTitle]", "Go");
                //Use HangFire
                BackgroundJob.Enqueue(() =>
                    emailSender.SendEmailAsync(user.Email, "Welcome Login", body));

                return View();
            }

            return PartialView("");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string code)
        {
            if (int.TryParse(code, out int intcode))
            {
                if (intcode == HttpContext.Session.GetInt32("ChangePassword"))
                {
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOldPassword(PasswordVM passwordVM)
        {
            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var result = await userManager.ChangePasswordAsync(user, passwordVM.CurrentPassword,
                    passwordVM.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Customer", new { id = user.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
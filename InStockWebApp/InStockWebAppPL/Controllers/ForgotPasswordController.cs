using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using InStockWebAppBLL.Models.ChangePasswordVM;
using InStockWebAppDAL.Entities;
using Hangfire;

namespace InStockWebAppPL.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IWebHostEnvironment webHostEnvironmen;

        public ForgotPasswordController(
            IEmailSender emailSender,
            UserManager<User> userManager,
            SignInManager<User> signInManager, IWebHostEnvironment webHostEnvironmen)
        {
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironmen=webHostEnvironmen;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                
                    return View("ForgotPasswordConfirmation");
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "ForgotPassword", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
                var filePath = $"{webHostEnvironmen.WebRootPath}/Account/Tempelet/Email.html";
                StreamReader str = new StreamReader(filePath);
                var body = str.ReadToEnd();
                str.Close();
                body = body.Replace("[Header]", "Welcom In Instock Shopping")
                    .Replace("[Body]", "Oops! It seems you've forgotten your password. No worries, let's get that sorted out.\r\n\r\nClick \"OK\" to proceed with resetting your password.")
                    .Replace("[URL]", $"{callbackUrl}")
                    .Replace("[AncorTitle]", "Go");
                //Use HangFire
                BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(model.Email, "Reset Password", body));
               

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string code)
        {
         
            var model = new ResetPasswordViewModel { UserId = userId, Code = code };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                
                return RedirectToAction("ForgotPassword", "ForgotPassword");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}

using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InStockWebAppBLL.Features.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using InStockWebAppBLL.Helpers.Role;

namespace InStockWebAppPL.Controllers
{
    public class AccountController : Controller
    {
        #region Prop
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegisterRepository registerRepo;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;
        #endregion


        #region Ctor
        public AccountController(IUserRepository userRepository, SignInManager<User> signInManager,
         UserManager<User> userManager, IUnitOfWork unitOfWork, IRegisterRepository registerRepo, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            this.registerRepo=registerRepo;
            this.emailSender=emailSender;
            this.webHostEnvironment=webHostEnvironment;
        }
        #endregion





        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await registerRepo.Register(model);

                    if (user != null)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

                        #region send Email
                        var filePath = $"{webHostEnvironment.WebRootPath}/Account/Tempelet/Email.html";
                        StreamReader str = new StreamReader(filePath);
                        var body = str.ReadToEnd();
                        str.Close();
                        body = body.Replace("[Header]", "Welcom In Instock Shopping")
                            .Replace("[Body]", "Welcome to InStock! Confirm Your Account")
                            .Replace("[URL]", $"{callbackUrl}")
                            .Replace("[AncorTitle]", "Confirm");
                        //Use HangFire
                        BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(model.Email, "Welcome Login", body));

                        #endregion

                        return View("Confirmation");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                            "ERROR: Failed to Register! Please try again!");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"ERROR: {ex.Message}");
            }

            return View(model);
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
          
            if (userId != null)
            {

                if(await _userRepository.ConfirmEmail(userId))
                return RedirectToAction("Login", "Account");
            }
           
            return RedirectToAction("Register");
        }
        #endregion


        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.FindByEmailAsync(loginVM.Email);
               
                if (user != null &&
                      user.EmailConfirmed&& await _userRepository.CheckPasswordAsync(user, loginVM.Password))
                {
                    var count = await _unitOfWork.CartRepository.GetCartItemsCount(user.Id);
                    HttpContext.Session.SetInt32("shoppingCartSession", count);

                    if (user.UserType == UserType.Customer)
                    {
                        await _signInManager.SignInAsync(user, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }


                    else if (user.UserType == UserType.Admin)
                    {
                        await _signInManager.SignInAsync(user, loginVM.RememberMe);
                        return RedirectToAction("Index", "Discount");
                    }
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(loginVM);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetInt32("shoppingCartSession", 0);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }


        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult ExternalLogin(string provider, string? returnURL = null)
        {
            var redirectURL = Url.Action("RegisterExternalUser", values: new { returnURL });
            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectURL);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterExternalUser(string? returnURL = null,
            string? remoteError = null)
        {
            returnURL = returnURL ?? Url.Content("~/");
            var message = "";

            if (remoteError != null)
            {
                message = $"Error from external provider: {remoteError}";
                return RedirectToAction("login", routeValues: new { message });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                message = "Error loading external login information.";
                return RedirectToAction("login", routeValues: new { message });
            }

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            // The account already exists
            if (externalLoginResult.Succeeded)
            {
                return LocalRedirect(returnURL);
            }

            string email = "";
            string firstName = "";
            string lastName = "";
            Gender gender = Gender.Male;
            string phoneNumber = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            }
            else
            {
                message = "Error while reading the email from the provider.";
                return RedirectToAction("login", routeValues: new { message });
            }

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
            }

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Surname))
            {
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }

            if (info.Principal.HasClaim(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender"))
            {
                var genderFromProvider =
                    info.Principal.FindFirstValue(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender")!;
                gender = MapGender(genderFromProvider);
            }

            if (info.LoginProvider == "Facebook" &&
                info.Principal.HasClaim(c => c.Type == "urn:facebook:gender"))
            {
                var genderFromProvider = info.Principal.FindFirstValue("urn:facebook:gender")!;
                gender = MapGender(genderFromProvider);
            }

            if (info.Principal.HasClaim(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"))
            {
                phoneNumber =
                    info.Principal.FindFirstValue(
                        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone")!;
            }

            var usuario = new User()
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                PhoneNumber = phoneNumber
            };

            usuario.CityId = 1;
            usuario.EmailConfirmed = true;

            var createUserResult = await _userManager.CreateAsync(usuario);
            if (!createUserResult.Succeeded)
            {
                message = createUserResult.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { message });
            }

            var addLoginResult = await _userManager.AddLoginAsync(usuario, info);

            if (addLoginResult.Succeeded)
            {
                await _signInManager.SignInAsync(usuario, isPersistent: false, info.LoginProvider);
                return LocalRedirect(returnURL);
            }

            message = "There was an error while logging you in.";
            return RedirectToAction("login", routeValues: new { message });
        }

        public Gender MapGender(string genderFromProvider)
        {
            return genderFromProvider?.ToLower() switch
            {
                "male" => Gender.Male,
                "female" => Gender.Female,
                _ => Gender.Male,
            };
        }
    }
}
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.UserVM;
using InStockWebAppDAL.Entities;
using InStockWebAppDAL.Entities.Enumerators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InStockWebAppPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(IUserRepository userRepository, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                model.CityId = 2;
                var user = await _userRepository.Register(model);

                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ERROR: Failed to Register! Please try again!");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult ExternalLogin(string provider, string? returnURL = null)
        {
            var redirectURL = Url.Action("RegisterExternalUser", values: new { returnURL });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectURL);
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

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

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

            if (info.Principal.HasClaim(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender"))
            {
                var genderFromProvider = info.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender")!;
                gender = MapGender(genderFromProvider);
            }
            if (info.LoginProvider == "Facebook" && info.Principal.HasClaim(c => c.Type == "urn:facebook:gender"))
            {
                var genderFromProvider = info.Principal.FindFirstValue("urn:facebook:gender")!;
                gender = MapGender(genderFromProvider);
            }

            if (info.Principal.HasClaim(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"))
            {
                phoneNumber = info.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone")!;
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

            usuario.CityId = 2;

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

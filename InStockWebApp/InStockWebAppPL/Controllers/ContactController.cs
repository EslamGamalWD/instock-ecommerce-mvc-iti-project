using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactMessageRepository contactMessage;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public ContactController(IContactMessageRepository contactMessage, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.contactMessage=contactMessage;
            this.userManager=userManager;
            this.signInManager=signInManager;
        }
        public async Task< IActionResult> Index()
        {

            var Message =await contactMessage?.GetAll();
            return View(Message);
        }


        public async Task<IActionResult>  Admin()
        {

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return View(await contactMessage.GetBySenderID(user.Id));
        }


        public async Task<IActionResult> Contact(string Id)
        {

            return View(await contactMessage.GetBySenderID(Id));

        }


        
    }
}

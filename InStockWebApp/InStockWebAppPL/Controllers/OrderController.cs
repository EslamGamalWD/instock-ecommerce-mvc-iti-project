using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Helpers.Role;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    [Authorize]

    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<User> userManager;

        public OrderController(IOrderRepository orderRepository ,UserManager <User> userManager)
        {
            this.orderRepository=orderRepository;
            this.userManager=userManager;
        }
        public async Task<IActionResult>  Index()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var orderResult =await orderRepository.GetAllOrders(user.Id);
            return View(orderResult);
        }
    }
}

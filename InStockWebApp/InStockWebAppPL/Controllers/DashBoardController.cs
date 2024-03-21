using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.DashBoadVM;
using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;

        public DashBoardController(IUserRepository userRepository,IProductRepository productRepository)
        {
            this.userRepository=userRepository;
            this.productRepository=productRepository;
        }
        public async Task<IActionResult> Index()
        {

            DashBoard dashBoard = new DashBoard()
            {
                TotalUser =  userRepository.getAll().Result.ToList().Count(),
                ProductSoldCount =await productRepository.GetAllProductSold()
            };
            return View(dashBoard);
        }
    }
}

using AutoMapper;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using InStockWebAppBLL.Models.UserVM;

namespace InStockWebAppPL.Controllers
{
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
            var user = await _userManager.GetUserAsync(HttpContext.User);
          
            if (user != null)
            {
                var userVM = _mapper.Map<GetUserByIdVM>(user);
                var userId = userVM.Id;
           
                return View("Details", userVM);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

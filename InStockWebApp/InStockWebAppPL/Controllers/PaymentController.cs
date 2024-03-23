using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppBLL.Models.AddressVM;
using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InStockWebAppBLL.Models.PaymentDetailesVM;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using InStockWebAppBLL.Features.Repositories.Domain;
using InStockWebAppDAL.Entities.Enumerators;
using InStockWebAppBLL.Models.OrderVM;

namespace InStockWebAppPL.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly IpaymentService _paymentService;
        private readonly UserManager<User> userManager;
        private readonly IItemRepository itemRepository;
        private readonly IPaymentDetailesRepository paymentDetailesRepository;
        private readonly IOrderRepository orderRepository;
        private readonly SignInManager<User> signInManager;

        public PaymentController(IConfiguration configuration , ICartRepository cartRepository , IpaymentService paymentService,UserManager<User> _userManager, IPaymentDetailesRepository paymentDetailesRepository, IOrderRepository orderRepository, SignInManager<User> signInManager, UserManager<User> userManager,IItemRepository itemRepository)
        {
            _configuration = configuration;
            _cartRepository = cartRepository;
            _paymentService = paymentService;
            userManager=_userManager;
            this.paymentDetailesRepository=paymentDetailesRepository;
            this.orderRepository=orderRepository;
            this.signInManager=signInManager;
            this.userManager=userManager;
            this.itemRepository=itemRepository;
        }
     
  
        public async Task<IActionResult> CreatePaymentSession(string userId)
        {
            if (userId != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync( await userManager.FindByIdAsync(userId));
                var callbackUrl = Url.Action("ConfirmPayment", "Payment", new { userId = userId, code = token }, protocol: HttpContext.Request.Scheme);

                var sessionUrl =  await  _paymentService.CreatePaymentSession(userId, callbackUrl);
                if (sessionUrl != null)
                    return Redirect(sessionUrl);
                else
                    return RedirectToAction("Index", "Home");


            }
            return RedirectToAction("Index", "Home");
        }
        //add Payment detailess
        //store order in Db
        //Decrease Unit Instock
        //incress units Sold
        //Reset Cart
        public async Task<IActionResult> ConfirmPayment(string userId, string code)
        {
            if (userId != null&&code !=null)
            {
                //add Payment detailess
                var cart = await _cartRepository.GetCart(userId);
                var PaymentId = await paymentDetailesRepository.Add(new AddPaymentDetailesVM() { Amount=cart.Items.Count(), PaymentStatus=PaymentStatus.Completed, CreatedAt=DateTime.Now, Provider="Stripe" });
                TempData["OrderList"]=cart;


                if (PaymentId !=null)
                {
                    TempData["PaymentId"] = PaymentId;
                return View();
                }
              
            }
            return RedirectToAction("Admin", "Contact");
        }
        [HttpPost]
        public async Task<IActionResult> Location(OrderAddress orderAddress)
        {
            //store order in Db
          
            var user =await userManager.FindByNameAsync(User.Identity.Name);
           
            var cart = await _cartRepository.GetCart(user.Id);

            AddOrderVM addOrderVM = new AddOrderVM()
            {
                CreatedAt = DateTime.Now,
                PaymentDetailsId =(int)TempData["PaymentId"],
                TotalPrice =cart.TotalPrice,
                UserId = user.Id,
            };
            //Decrease Unit Instock
            //incress units Sold
            var data = await orderRepository.Add(addOrderVM);
            if (data !=null)
            {
              await  itemRepository.DecreaseUnitStock(cart.Id, data);
              await  itemRepository.IncreaseUnitSold(cart.Id);
            }
            //Reset Cart
            await _cartRepository.EmptyCart(cart.Id);
            TempData["SucessPayment"] ="sucess";
            return RedirectToAction("Index","Cart");
        }

    }
    
}

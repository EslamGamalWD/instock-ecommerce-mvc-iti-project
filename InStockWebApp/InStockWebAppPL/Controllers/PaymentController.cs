using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppBLL.Features.Interfaces.Domain;

using InStockWebAppDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;

namespace InStockWebAppPL.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly IpaymentService _paymentService;

        public PaymentController(IConfiguration configuration , ICartRepository cartRepository , IpaymentService paymentService)
        {
            _configuration = configuration;
            _cartRepository = cartRepository;
            _paymentService = paymentService;

        }
     
  
        public async Task<IActionResult> CreatePaymentSession(string userId)
        {
            if (userId != null)
            {
                var sessionUrl =  await  _paymentService.CreatePaymentSession(userId);
                if (sessionUrl != null)
                    return Redirect(sessionUrl);
                else
                    return RedirectToAction("Index", "Home");


            }
            return RedirectToAction("Index", "Home");
        }

    }
    
}

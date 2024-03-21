using Microsoft.AspNetCore.Mvc;

namespace InStockWebAppPL.Controllers
{
    public class FooterController : Controller
    {
       public  IActionResult ReturnsAndRefunds()
        {
            return View();
        }
        public IActionResult TermsConditions()
        {
            return View();
        }
        public IActionResult WhatDoWeDo()
        {
            return View();
        }
        public IActionResult AvailableServices()
        {
            return View();
        }
        public IActionResult FAQS()
        {
            return View();
        }
    }
}

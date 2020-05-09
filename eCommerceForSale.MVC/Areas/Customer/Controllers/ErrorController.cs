using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceForSale.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ErrorController : Controller
    {
        [Route("/Error/{code}")]
        public IActionResult HttpStatusCodeHandler(int code)
        {
            switch (code)
            {
                case 404:
                    ViewBag.ErrorMessage = Constants.ErrorMessage;
                    break;
            }
            return View("NotFound");
        }
    }
}
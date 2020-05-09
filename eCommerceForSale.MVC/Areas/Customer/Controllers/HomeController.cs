using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace eCommerceForSale.MVC.Area.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _unitOfWork.Product.GetAll();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var cartCount = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId.Equals(claim.Value)).Result.Count();
                HttpContext.Session.SetInt32(Constants.ShoppingCartSession, cartCount);
            }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Product(Guid id)
        {
            var productDb = _unitOfWork.Product.GetFirstOfDefault(x => x.Id.Equals(id));
            if (productDb == null)
            {
                return NotFound();
            }
            var shoppingCart = new ShoppingCart
            {
                Product = productDb,
                ProductId = productDb.Id,
            };
            var productDetailsVM = new ProductDetailsVM
            {
                ShoppingCart = shoppingCart,
                PriceForWeight = _unitOfWork.PriceForWeight.GetAll(x => x.ProductId.Equals(id)).Result
            };

            return View(productDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Product")]
        [Authorize]
        public IActionResult ProductPost(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = new Guid();
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                shoppingCart.ApplicationUserId = claim.Value;
                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOfDefault(
                    x => x.ApplicationUserId.Equals(shoppingCart.ApplicationUserId) && x.ProductId.Equals(shoppingCart.ProductId), isIncludeProperties: "Product");

                if (cartFromDb == null)
                {
                    shoppingCart.Price = GetPriceForTheProduct(shoppingCart.ProductId, shoppingCart.Count, shoppingCart.PriceForWeight);
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    shoppingCart.Count += cartFromDb.Count;
                    shoppingCart.Price = GetPriceForTheProduct(shoppingCart.ProductId, shoppingCart.Count, shoppingCart.PriceForWeight);
                    _unitOfWork.ShoppingCart.Update(shoppingCart);
                }
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId.Equals(shoppingCart.ApplicationUserId)).Result;
                HttpContext.Session.SetInt32(Constants.ShoppingCartSession, count.Count());
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productDetailsVM = new ProductDetailsVM
                {
                    ShoppingCart = shoppingCart,
                    PriceForWeight = _unitOfWork.PriceForWeight.GetAll(x => x.ProductId.Equals(shoppingCart.ProductId)).Result
                };

                return View(productDetailsVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GlobalProductSearch([FromBody]string name)
        {
            var productNames = new List<string>();
            productNames = _unitOfWork.Product.GetAll(p => p.ProductName.StartsWith(name)).Result.Select(p => p.ProductName).ToList();
            return Json(new { data = productNames });
        }

        private double GetPriceForTheProduct(Guid productId, int count, PriceForWeight priceForWeight = null)
        {
            double price = 0.00;
            if (priceForWeight == null)
            {
                var productPrice = _unitOfWork.Product.GetFirstOfDefault(x => x.Id.Equals(productId)).Price;
                price = productPrice * count;
            }
            return price;
        }

        [Route("Customer/Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = Constants.ErrorMessage;
                    break;
            }
            return View("NotFound");
        }
    }
}
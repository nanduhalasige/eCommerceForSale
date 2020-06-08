using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Entity.ViewModels;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace eCommerceForSale.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailOptions _emailOptions;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork, IOptions<EmailOptions> options, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailOptions = options.Value;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            Claim claims = GetClaims();
            ShoppingCartVM = new ShoppingCartVM
            {
                OrderHeader = new Entity.Models.OrderHeader(),
                CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId.Equals(claims.Value), isIncludeProperties: "Product").Result
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOfDefault(x => x.Id.Equals(claims.Value));

            foreach (var item in ShoppingCartVM.CartList)
            {
                item.Price = GetPriceForTheProduct(item.ProductId, item.Count, null);
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price;
                item.Product.Desciption = Constants.ConvertToRawHtml(item.Product.Desciption);
                if (item.Product.Desciption.Length > 100)
                {
                    item.Product.Desciption = item.Product.Desciption.Substring(0, 99) + "...";
                }
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Minus(Guid cartId)
        {
            var cartObj = _unitOfWork.ShoppingCart.GetFirstOfDefault(x => x.Id.Equals(cartId), isIncludeProperties: "Product");
            if (cartObj.Count > 0)
            {
                cartObj.Count -= 1;
                cartObj.Price = GetPriceForTheProduct(cartObj.ProductId, cartObj.Count, null);
            }
            else
            {
                _unitOfWork.ShoppingCart.Remove(cartId);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Plus(Guid cartId)
        {
            var cartObj = _unitOfWork.ShoppingCart.GetFirstOfDefault(x => x.Id.Equals(cartId), isIncludeProperties: "Product");
            if (cartObj.Count < cartObj.Product.Stock)
            {
                cartObj.Count += 1;
                cartObj.Price = GetPriceForTheProduct(cartObj.ProductId, cartObj.Count, null);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveProduct(Guid cartId)
        {
            var cartObj = _unitOfWork.ShoppingCart.GetFirstOfDefault(x => x.Id.Equals(cartId));
            if (cartObj != null)
            {
                _unitOfWork.ShoppingCart.Remove(cartId);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CartSummary()
        {
            Claim claims = GetClaims();
            ShoppingCartVM = new ShoppingCartVM
            {
                OrderHeader = new OrderHeader(),
                Addresses = new List<Address>(),
                CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId.Equals(claims.Value), isIncludeProperties: "Product").Result
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOfDefault(u => u.Id.Equals(claims.Value));

            foreach (var item in ShoppingCartVM.CartList)
            {
                item.Price = GetPriceForTheProduct(item.ProductId, item.Count, null);
                ShoppingCartVM.OrderHeader.OrderTotal += item.Price;
            }
            var address = _unitOfWork.Address.GetAll(a => a.ApplicationUserId.Equals(claims.Value)).Result;
            if (address.Count() > 0)
            {
                ShoppingCartVM.Addresses = address.OrderByDescending(a => a.IsDefault).ThenBy(a => a.FullName);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CartSummary")]
        public IActionResult CartSummaryPost(string stripeToken)
        {
            if (stripeToken == null)
            {
                return RedirectToAction(nameof(CartSummary));
            }
            else
            {
                Claim claims = GetClaims();
                ShoppingCartVM.OrderHeader = new OrderHeader();
                ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOfDefault(u => u.Id.Equals(claims.Value));
                ShoppingCartVM.CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId.Equals(claims.Value), isIncludeProperties: "Product").Result;

                ShoppingCartVM.OrderHeader.PaymentStatus = Constants.StatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = Constants.StatusPending;
                ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;
                ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

                //adding address
                ShoppingCartVM.OrderHeader.AddressId = ShoppingCartVM.AddressId;

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                var orderDetailsList = new List<OrderDetails>();
                foreach (var item in ShoppingCartVM.CartList)
                {
                    var orderDetail = new OrderDetails
                    {
                        ProductId = item.ProductId,
                        OrderId = ShoppingCartVM.OrderHeader.Id,
                        Price = item.Product.Price.ToString(),
                        Count = item.Count
                    };

                    ShoppingCartVM.OrderHeader.OrderTotal += item.Count * item.Product.Price;

                    _unitOfWork.OrderDetails.Add(orderDetail);
                    _unitOfWork.ShoppingCart.Remove(item.Id);
                }
                HttpContext.Session.SetInt32(Constants.ShoppingCartSession, 0);
                _unitOfWork.Save();

                var option = new Stripe.ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "pln",
                    Description = "Order Id: " + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };
                var service = new Stripe.ChargeService();
                Stripe.Charge charge = service.Create(option);
                if (charge.BalanceTransactionId == null)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = Constants.StatusRejected;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.TansactionId = charge.BalanceTransactionId;
                }

                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = Constants.StatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = Constants.StatusConfirmed;
                    ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                }
            }
            _unitOfWork.Save();
            return RedirectToAction("OrderConfirmation", "ShoppingCart", new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(Guid id)
        {
            var confirmedOrder = _unitOfWork.OrderHeader.GetFirstOfDefault(o => o.Id.Equals(id), isIncludeProperties: "ApplicationUser,Address");
            _emailSender.SendEmailAsync(confirmedOrder.ApplicationUser.Email, "Order Confirmation",
                $"Successfull... Your order hase been placed");

            return View(confirmedOrder);
        }

        public IActionResult AddOrModifyAddress()
        {
            var address = new Address();
            return PartialView("_AddressModelFormContent", address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrModifyAddress(Address address)
        {
            Claim claim = GetClaims();
            if (ModelState.IsValid)
            {
                address.ApplicationUserId = claim.Value;
                var addressDb = _unitOfWork.Address.GetFirstOfDefault(a => a.ApplicationUserId.Equals(claim.Value) && a.IsDefault);

                //make first address defaulst
                if (addressDb == null)
                {
                    address.IsDefault = true;
                }
                else
                {
                    if (address.IsDefault)
                    {
                        addressDb.IsDefault = false;
                    }
                }
                _unitOfWork.Address.Add(address);
                _unitOfWork.Save();
            }

            return PartialView("_AddressModelFormContent", address);
        }

        /// <summary>
        /// Gives logged in user claims
        /// </summary>
        /// <returns></returns>
        private Claim GetClaims()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claims;
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
    }
}
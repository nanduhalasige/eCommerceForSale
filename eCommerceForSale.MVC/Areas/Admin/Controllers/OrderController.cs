using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using eCommerceForSale.Entity.ViewModels;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace eCommerceForSale.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderHeaderVM OrderHeaderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            var orders = _unitOfWork.OrderHeader.GetAll(isIncludeProperties: "ApplicationUser,Address").Result;
            return orders;
        }

        public IActionResult EditOrder(Guid Id)
        {
            OrderHeaderVM = new OrderHeaderVM
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOfDefault(o => o.Id.Equals(Id), isIncludeProperties: "ApplicationUser,Address"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(d => d.OrderId.Equals(Id), isIncludeProperties: "Product").Result
            };
            if (OrderHeaderVM == null)
            {
                return NotFound();
            }
            return View(OrderHeaderVM);
        }

        [HttpPost]
        public IActionResult EditOrder()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOfDefault(o => o.Id.Equals(OrderHeaderVM.OrderHeader.Id), isIncludeProperties: "ApplicationUser,Address");
            //var addressDb = _unitOfWork.Address.GetFirstOfDefault(o => o.Id.Equals(OrderHeaderVM.OrderHeader.AddressId));
            if (orderHeaderDb == null)
            {
                return NotFound();
            }
            try
            {
                orderHeaderDb.OrderStatus = OrderHeaderVM.OrderHeader.OrderStatus;
                orderHeaderDb.PaymentStatus = OrderHeaderVM.OrderHeader.PaymentStatus;
                orderHeaderDb.ShippingDate = OrderHeaderVM.OrderHeader.ShippingDate;
                orderHeaderDb.TrackingNumber = OrderHeaderVM.OrderHeader.TrackingNumber;
                orderHeaderDb.Carrier = OrderHeaderVM.OrderHeader.Carrier;

                orderHeaderDb.Address.AddressLine1 = OrderHeaderVM.OrderHeader.Address.AddressLine1;
                orderHeaderDb.Address.AddressLine2 = OrderHeaderVM.OrderHeader.Address.AddressLine2;
                orderHeaderDb.Address.PostCode = OrderHeaderVM.OrderHeader.Address.PostCode;
                orderHeaderDb.Address.MobileNumber = OrderHeaderVM.OrderHeader.Address.MobileNumber;
                _unitOfWork.Save();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Constants.AdminRole + "," + Constants.EmployeeRole)]
        public IActionResult StartProcessing(Guid Id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOfDefault(o => o.Id.Equals(Id));
            orderHeader.OrderStatus = Constants.StatusProcessing;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Constants.AdminRole + "," + Constants.EmployeeRole)]
        public IActionResult ShipOrder(Guid Id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOfDefault(o => o.Id.Equals(Id));
            orderHeader.TrackingNumber = OrderHeaderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderHeaderVM.OrderHeader.Carrier;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Constants.AdminRole + "," + Constants.EmployeeRole)]
        public IActionResult CancelOrder(Guid Id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOfDefault(o => o.Id.Equals(Id));
            if (orderHeader.PaymentStatus == Constants.StatusApproved)
            {
                var option = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TansactionId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);

                orderHeader.OrderStatus = Constants.StatusRefunded;
                orderHeader.PaymentStatus = Constants.StatusRefunded;
            }
            else
            {
                orderHeader.OrderStatus = Constants.StatusCancelled;
                orderHeader.PaymentStatus = Constants.StatusCancelled;
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
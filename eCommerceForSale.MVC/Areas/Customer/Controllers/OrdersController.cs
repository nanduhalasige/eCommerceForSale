using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceForSale.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var OrderHeaderListVM = new List<OrderHeaderVM>();
            var orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId.Equals(claim.Value), isIncludeProperties: "ApplicationUser,Address").Result;
            if (orderHeaders != null)
            {
                foreach (var orderHeader in orderHeaders)
                {
                    OrderHeaderListVM.Add(new OrderHeaderVM
                    {
                        OrderHeader = orderHeader,
                        OrderDetails = _unitOfWork.OrderDetails.GetAll(d => d.OrderId.Equals(orderHeader.Id), isIncludeProperties: "Product").Result
                    });
                }
            }
            return View(OrderHeaderListVM.AsEnumerable());
        }
    }
}
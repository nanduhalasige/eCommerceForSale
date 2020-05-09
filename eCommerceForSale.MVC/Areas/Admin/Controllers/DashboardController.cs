using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.ViewModels;
using eCommerceForSale.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eCommerceForSale.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRole + "," + Constants.EmployeeRole)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllDashboardData()
        {
            var dashboardVm = new DashboardVM
            {
                Sales = new List<Sales>()
            };
            //o => o.OrderStatus.Equals(Constants.StatusCompleted) && o.PaymentStatus.Equals(Constants.StatusConfirmed)
            var saleData = _unitOfWork.OrderHeader.GetAll().Result;

            foreach (var sale in saleData)
            {
                var Sale = new Sales
                {
                    SaleId = sale.Id,
                    DateOfSale = sale.PaymentDate,
                    SaleValue = sale.OrderTotal
                };
                dashboardVm.Sales.Add(Sale);
            }
            return Json(new { data = dashboardVm });
        }
    }
}
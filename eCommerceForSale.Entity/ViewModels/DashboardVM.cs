using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Entity.ViewModels
{
    public class DashboardVM
    {
        public List<Sales> Sales { get; set; }
    }

    public class Sales
    {
        public Guid SaleId { get; set; }
        public DateTime DateOfSale { get; set; }
        public double SaleValue { get; set; }
    }
}
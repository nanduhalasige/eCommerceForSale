using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Entity.ViewModels
{
    public class HomeVM
    {
        public string SerachString { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
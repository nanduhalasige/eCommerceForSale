using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Entity.ViewModels
{
    public class ProductDetailsVM
    {
        public IEnumerable<PriceForWeight> PriceForWeight { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        //public OrderDetails OrderDetails { get; set; }
    }
}
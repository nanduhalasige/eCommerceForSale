using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Entity.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}
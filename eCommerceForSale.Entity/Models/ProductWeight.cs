using System;

namespace eCommerceForSale.Entity.Models
{
    public class ProductWeight
    {
        public Guid Id { get; set; }
        public int Weight { get; set; }
        public string WeightUnit { get; set; }
    }
}
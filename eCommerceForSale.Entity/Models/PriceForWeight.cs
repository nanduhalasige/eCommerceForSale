using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceForSale.Entity.Models
{
    public class PriceForWeight
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid WeightId { get; set; }

        [ForeignKey("WeightId")]
        public ProductWeight ProductWeight { get; set; }

        public int Stock { get; set; }
        public double Price { get; set; }
    }
}
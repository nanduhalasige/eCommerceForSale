using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eCommerceForSale.Entity.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }

        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "Count cannot be outside the range 1 to 1000")]
        public int Count { get; set; }

        [NotMapped]
        public double Price { get; set; }

        public Guid? WeightPriceId { get; set; }

        [ForeignKey("WeightPriceId")]
        public PriceForWeight PriceForWeight { get; set; }
    }
}
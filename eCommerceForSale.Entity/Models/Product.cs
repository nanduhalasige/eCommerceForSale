using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceForSale.Entity.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name is essential")]
        [MaxLength(100, ErrorMessage = "Product name cannot have more than 100 character")]
        public string ProductName { get; set; }

        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "Product description is essential")]
        public string Desciption { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Category of the product")]
        [Required(ErrorMessage = "Please select category")]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public int Stock { get; set; }

        [Display(Name = "Sold in box or weights")]
        public bool IsBoxOrPack { get; set; }

        public int Price { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
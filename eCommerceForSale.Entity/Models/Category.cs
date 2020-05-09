using System;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace eCommerceForSale.Entity.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category name is essential")]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public bool IsActive { get; set; }
    }
}
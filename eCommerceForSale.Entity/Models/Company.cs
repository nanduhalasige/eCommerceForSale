using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eCommerceForSale.Entity.Models
{
    public class Company
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Cannot proceed without Company name")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string LogoPath { get; set; }

        [Required(ErrorMessage = "Name of the owener is essential")]
        [Display(Name = "Owener Name")]
        public string OwenerName { get; set; }

        [Required(ErrorMessage = "Company Email address is required")]
        [Display(Name = "Company Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email id")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Tax Number")]
        public string TaxNumber { get; set; }
    }
}
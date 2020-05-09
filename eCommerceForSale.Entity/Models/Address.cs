using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eCommerceForSale.Entity.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "Name is required to deliver the product")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "House No. and locality seems essential")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "City and State is also essential")]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Postal code required")]
        [MaxLength(6)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal Code")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Essential info for contacting")]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Make it default address")]
        public bool IsDefault { get; set; }
    }
}
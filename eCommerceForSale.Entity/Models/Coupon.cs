using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eCommerceForSale.Entity.Models
{
    public class Coupon
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Coupon code is mandatory")]
        [Display(Name = "Coupon code")]
        public string CouponCode { get; set; }

        [Display(Name = "Valid till")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        public DateTime ValidTill { get; set; }

        public Double Discount { get; set; }
        public bool IsPercent { get; set; }
        public bool IsActive { get; set; }
    }
}
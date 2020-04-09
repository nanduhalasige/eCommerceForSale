using eCommerceForSale.Entity.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eCommerceForSale.Entity.ViewModels
{
    public class CompanyView
    {
        public Company Company { get; set; }
        public bool EnableControls { get; set; }

        [Display(Name = "Logo of the company")]
        public IFormFile Logofile { get; set; }
    }
}
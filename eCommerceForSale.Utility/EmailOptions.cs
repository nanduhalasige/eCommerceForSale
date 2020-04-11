using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Utility
{
    public class EmailOptions
    {
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string FromName { get; set; }
    }
}
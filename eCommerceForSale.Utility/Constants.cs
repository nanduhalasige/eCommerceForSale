using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Utility
{
    public class Constants
    {
        public const string AdminRole = "Administrator";
        public const string CustomerRole = "Customer";
        public const string EmployeeRole = "Employee";
        public const string ErrorMessage = "Oops... The resource you requested not found";

        public const string ShoppingCartSession = "Shopping cart session";
        public const string NameOfUser = "Name of the user";

        public static string StatusPending = "Pending";
        public static string StatusRejected = "Rejected";
        public static string StatusApproved = "Approved";
        public static string StatusConfirmed = "Confirmed";
        public static string StatusProcessing = "Processing";
        public static string StatusCancelled = "Cancelled";
        public static string StatusRefunded = "Refunded";
        public static string StatusShipped = "Shipped";
        public static string StatusCompleted = "Completed";

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
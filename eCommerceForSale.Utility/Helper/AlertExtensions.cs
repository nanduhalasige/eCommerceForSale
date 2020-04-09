using Microsoft.AspNetCore.Mvc;

namespace eCommerceForSale.Utility.Helper
{
    public static class AlertExtensions
    {
        public static IActionResult WithSuccess(this IActionResult result, string message)
        {
            return Alert(result, "success", message);
        }

        public static IActionResult WithInfo(this IActionResult result, string message)
        {
            return Alert(result, "info", message);
        }

        public static IActionResult WithWarning(this IActionResult result, string message)
        {
            return Alert(result, "warning", message);
        }

        public static IActionResult WithDanger(this IActionResult result, string message)
        {
            return Alert(result, "danger", message);
        }

        private static IActionResult Alert(IActionResult result, string type, string message)
        {
            return new AlertDecoratorResult(result, type, message);
        }
    }
}
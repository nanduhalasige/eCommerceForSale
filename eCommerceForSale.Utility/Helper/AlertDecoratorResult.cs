using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceForSale.Utility.Helper
{
    public class AlertDecoratorResult : IActionResult
    {
        public IActionResult Result { get; }
        public string Type { get; }
        public string Message { get; }

        public AlertDecoratorResult(IActionResult result, string type, string message)
        {
            Result = result;
            Type = type;
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            //NOTE: Be sure you add a using statement for Microsoft.Extensions.DependencyInjection, otherwise
            //      this overload of GetService won't be available!
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = Type;
            tempData["_alert.message"] = Message;

            await Result.ExecuteResultAsync(context);
        }
    }
}
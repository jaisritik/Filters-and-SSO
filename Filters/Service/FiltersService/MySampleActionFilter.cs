using Microsoft.AspNetCore.Mvc.Filters;
using System.Xml.Linq;

namespace Filters.Service.FiltersService
{
    public class MySampleActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"OnActionExecuted Globle");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"OnActionExecuted Globle");
        }
    }
}

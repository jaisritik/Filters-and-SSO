﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Service.FiltersService
{
    public class MySampleAsyncActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _name;

        public MySampleAsyncActionFilterAttribute(string name)
        {
            _name = name;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"Before Async Execution {_name}");
            await next();
            Console.WriteLine($"After Async Execution {_name}");
        }
    }
}

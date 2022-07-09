using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.API.Helpers
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class KeyMustBeProvided : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Query.ContainsKey("key"))
            {
                context.Result = new BadRequestObjectResult("The 'Key' parameter was not supplied")
                {
                    StatusCode = 400
                };
                return;
            }
            if (string.IsNullOrEmpty(context.HttpContext.Request.Query.Keys.Where(x=> x == "key").FirstOrDefault() ?? "" ))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "The 'Key' parameter was empty"
                };
                return;
            }

            await next();
        }
    }
}

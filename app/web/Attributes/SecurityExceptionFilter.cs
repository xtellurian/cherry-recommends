
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SignalBox.Core;

namespace SignalBox.Web
{
    /// <summary>
    /// Catches thrown SecurityException and returns an HTTP 401 Unauthorized response.
    /// </summary>
    public class SecurityExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<SecurityExceptionFilter> logger;

        public SecurityExceptionFilter(ILogger<SecurityExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is SecurityException ex)
            {
                logger.LogError(ex, "Unauthorized");
                // Generated response uses ProblemDetails format
                context.ExceptionHandled = true;
                context.Result = new UnauthorizedObjectResult(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
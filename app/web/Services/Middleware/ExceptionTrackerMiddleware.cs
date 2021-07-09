using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using System.Globalization;
using System.Threading.Tasks;

namespace SignalBox.Web.Services
{
    public class ExceptionTelemetryMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionTelemetryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var telemetry = context.RequestServices.GetService<ITelemetry>();
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                telemetry.TrackException(ex);
                throw;
            }
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SignalBox.Client
{
    internal class SignalBoxMiddleware
    {
        private readonly RequestDelegate _next;

        public SignalBoxMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, SignalBoxClient client)
        {
            if (context.Request.Path == "/_SignalBox")
            {
                await context.Response.WriteAsJsonAsync(await client.GetJavascriptConfiguration());
            }

            // Call the next delegate/middleware in the pipeline
            // await _next(context);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Web.Services
{
    public class TenantSelectorMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantSelectorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var isAllowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object;

            var tenantProvider = context.RequestServices.GetRequiredService<ITenantProvider>();
            var resolver = context.RequestServices.GetRequiredService<ITenantResolutionStrategy>();

            var req = new HttpRequestModel(context.Request.Path.Value.ToString(),
                context.Request.Headers.ToDictionary(_ => _.Key, _ => _.Value.ToString()));
            // this should always be set
            await tenantProvider.SetTenantName(await resolver.ResolveName(req));

            // only try to authorize the request if the server is multitenant and the request isnt anonymous
            if (!isAllowAnonymous && resolver.IsMultitenant)
            {
                var tenant = tenantProvider.Current();
                var authorizor = context.RequestServices.GetRequiredService<ITenantAuthorizationStrategy>();
                await authorizor.Authorize(context.User, tenant);
            }

            await _next(context);
        }
    }
}
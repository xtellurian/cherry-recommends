using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;

namespace SignalBox.Web
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private const string apiKeyQueryStringName = "apiKey";
        private const string apiKeyHeaderName = "x-api-key";

        private bool TryGetApiKey(HttpContext context, out string key)
        {
            if (context.Request.Query.TryGetValue(apiKeyQueryStringName, out var qsKey))
            {
                key = qsKey;
                return true;
            }
            else if (context.Request.Headers.TryGetValue(apiKeyHeaderName, out var headerKey))
            {
                key = headerKey;
                return true;
            }
            else
            {
                key = null;
                return false;
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var tenantProvider = context.RequestServices.GetService<ITenantProvider>();
            var logger = context.RequestServices.GetService<ILogger<ApiKeyMiddleware>>();
            var ApiKeyAttributeObject = endpoint?.Metadata?.GetMetadata<AllowApiKeyAttribute>();
            if (ApiKeyAttributeObject is AllowApiKeyAttribute attribute)
            {
                logger.LogInformation("Endpoint allows ApiKey");
                // load the API key store
                if (!context.User.Identity.IsAuthenticated && TryGetApiKey(context, out var key))
                {
                    var workflows = context.RequestServices.GetRequiredService<ApiKeyWorkflows>();
                    if (await workflows.IsValidApiKey(key, attribute.KeyType))
                    {
                        var record = await workflows.LoadRecord(key);
                        var identity = new GenericIdentity(record.Name, "api-key");
                        var tenant = tenantProvider.Current();
                        if (tenant != null)
                        {
                            identity.AddClaim(new Claim("scope", tenant.AccessScope()));
                        }
                        context.User = new ClaimsPrincipal(identity);
                    }
                }
            }
            else
            {
                logger.LogInformation("Endpoint denies ApiKey");
            }

            await _next(context);
        }
    }
}
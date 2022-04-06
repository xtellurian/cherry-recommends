using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Web.Services
{
#nullable enable
    public class HttpHeaderTenantResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly Hosting hosting;

        public HttpHeaderTenantResolutionStrategy(IOptions<Hosting> options)
        {
            this.hosting = options.Value;
        }

        public bool IsMultitenant => true;

        public Task<string?> ResolveName(HttpRequestModel request)
        {
            if (hosting.Multitenant)
            {
                if (request.Headers.TryGetValue("x-tenant", out var tenantName))
                {
                    return Task.FromResult<string?>(tenantName);
                }
            }

            // default return null
            return Task.FromResult<string?>(null);
        }
    }
}
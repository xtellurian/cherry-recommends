using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Web.Services
{
#nullable enable
    public class SubdomainTenantResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly Hosting hosting;

        public SubdomainTenantResolutionStrategy(IOptions<Hosting> options)
        {
            this.hosting = options.Value;
        }

        public bool IsMultitenant => true;

        public Task<string?> ResolveName(HttpRequestModel request)
        {
            if (hosting.Multitenant)
            {
                if (request.Headers.TryGetValue("Host", out var host))
                {
                    if (!host.Contains(hosting.CanonicalRootDomain))
                    {
                        return Task.FromResult<string?>(null);
                    }
                    string? sub = host.Replace(this.hosting.CanonicalRootDomain, "").Trim().Trim('.');
                    return Task.FromResult<string?>(sub);
                }
                else if (request.Headers.TryGetValue("host", out var hostLower))
                {
                    string? sub = hostLower.Split('.').First();
                    return Task.FromResult<string?>(sub);
                }
                else
                {
                    return Task.FromResult<string?>(null);
                }
            }
            else
            {
                return Task.FromResult<string?>(null);
            }
        }
    }
}
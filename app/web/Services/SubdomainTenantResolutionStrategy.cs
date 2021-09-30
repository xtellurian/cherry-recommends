using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Web.Services
{
    public class SubdomainTenantResolutionStrategy : ITenantResolutionStrategy
    {
        private Hosting hosting;

        public SubdomainTenantResolutionStrategy(IOptions<Hosting> options)
        {
            this.hosting = options.Value;
        }

        public Task<string> ResolveName(HttpRequestModel request)
        {
            if (hosting.Multitenant)
            {
                if (request.Headers.TryGetValue("Host", out var host))
                {
                    if (!host.Contains(hosting.CanonicalRootDomain))
                    {
                        throw new BadRequestException($"Host: {host} does not contain canonical root domain {hosting.CanonicalRootDomain}");
                    }
                    var sub = host.Replace(this.hosting.CanonicalRootDomain, "").Trim().Trim('.');
                    return Task.FromResult(sub);
                }
                else if (request.Headers.TryGetValue("host", out var hostLower))
                {
                    var sub = hostLower.Split('.').First();
                    return Task.FromResult(sub);
                }
                else
                {
                    throw new BadRequestException("Host header not found");
                }
            }
            else
            {
                return null;
            }
        }
    }
}
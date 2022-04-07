using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Web.Services
{
#nullable enable
    public class MultiMethodTenantResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly SubdomainTenantResolutionStrategy subdomainStrategy;
        private readonly HttpHeaderTenantResolutionStrategy headerStrategy;
        private readonly QueryTenantResolutionStrategy queryStrategy;

        public MultiMethodTenantResolutionStrategy(IOptions<Hosting> options)
        {
            this.subdomainStrategy = new SubdomainTenantResolutionStrategy(options);
            this.headerStrategy = new HttpHeaderTenantResolutionStrategy(options);
            this.queryStrategy = new QueryTenantResolutionStrategy(options);
        }

        public bool IsMultitenant => true;

        public async Task<string?> ResolveName(HttpRequestModel request)
        {
            var name = await subdomainStrategy.ResolveName(request); // subdomain takes priority
            if (string.IsNullOrEmpty(name))
            {
                name = await headerStrategy.ResolveName(request);
            }
            if (string.IsNullOrEmpty(name))
            {
                name = await queryStrategy.ResolveName(request);
            }

            // default return null
            return name;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;

namespace SignalBox.Web.Services
{
    public class SingleTenantResolverStrategy : ITenantResolutionStrategy
    {
        private readonly ILogger<SingleTenantResolverStrategy> logger;

        public SingleTenantResolverStrategy(ILogger<SingleTenantResolverStrategy> logger)
        {
            this.logger = logger;
        }

        public bool IsMultitenant => false;

        public Task<string> ResolveName(HttpRequestModel reqest)
        {
            logger.LogWarning("Single tenant resolver will resolve null");
            return Task.FromResult<string>(null);
        }
    }
}
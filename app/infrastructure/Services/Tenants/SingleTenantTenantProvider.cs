using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Infrastructure.Services
{
    public class SingleTenantTenantProvider : ITenantProvider
    {
        private readonly ILogger<SingleTenantTenantProvider> logger;
        private readonly Hosting hosting;
        private string name;

        public SingleTenantTenantProvider(IOptions<Hosting> hostingOptions, ILogger<SingleTenantTenantProvider> logger)
        {
            this.logger = logger;
            this.hosting = hostingOptions.Value;
            if (string.IsNullOrEmpty(this.hosting.SingleTenantDatabaseName))
            {
                logger.LogError("Hosting:SingleTenantDatabaseName is null");
            }
            else
            {
                logger.LogInformation($"Hosting:SingleTenantDatabaseName is {this.hosting.SingleTenantDatabaseName}");
            }
        }

        public string RequestedTenantName => this.name;

        public Tenant Current()
        {
            logger.LogWarning("Accessing tenant in SingleTenant Tenant Provider is not recommended");
            return null;
        }

        public string CurrentDatabaseName => this.hosting.SingleTenantDatabaseName;

        public Task SetTenantName(string name)
        {
            this.name = name;
            return Task.CompletedTask;
        }
    }
}
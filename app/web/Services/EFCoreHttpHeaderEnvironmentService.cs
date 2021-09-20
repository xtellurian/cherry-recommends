using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Web.Services
{
#nullable enable
    public class EFCoreHttpHeaderEnvironmentService : IEnvironmentService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<EFCoreHttpHeaderEnvironmentService> logger;

        public long? CurrentEnvironmentId => this.EnvironmentId();

        public EFCoreHttpHeaderEnvironmentService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<EFCoreHttpHeaderEnvironmentService> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        private long? EnvironmentId()
        {
            if (this.httpContextAccessor.HttpContext == null)
            {
                logger.LogWarning("Tried to get the EnvironmentId without an HttpContext.");
                return null;
            }
            if (this.httpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-environment", out var headerValue))
            {
                var stringValue = headerValue.FirstOrDefault();
                if (!string.IsNullOrEmpty(stringValue) && long.TryParse(stringValue, out var envId))
                {
                    logger.LogInformation($"Environment Id is {envId}.");
                    return envId;
                }
            }

            logger.LogInformation("Environment Id is null. Using default environment");
            return null;
        }

        public async Task<Environment?> ReadCurrent(IEnvironmentStore store)
        {
            var id = this.CurrentEnvironmentId;
            if (id.HasValue)
            {
                return await store.Read(id.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
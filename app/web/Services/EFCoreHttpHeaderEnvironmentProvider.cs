using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Web.Services
{
#nullable enable
    public class EFCoreHttpHeaderEnvironmentProvider : IEnvironmentProvider, IInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<EFCoreHttpHeaderEnvironmentProvider> logger;

        public long? CurrentEnvironmentId => this.EnvironmentId();

        public EFCoreHttpHeaderEnvironmentProvider(
            IHttpContextAccessor httpContextAccessor,
            ILogger<EFCoreHttpHeaderEnvironmentProvider> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        private long? _overrideEnvironmentId;
        private long? EnvironmentId()
        {
            if(_overrideEnvironmentId != null)
            {
                return _overrideEnvironmentId;
            }
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

        // this can be called from within a store, so the IEnvironmentStore should be passed in.
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

        public Task SetOverride(long environmentId)
        {
            _overrideEnvironmentId = environmentId;
            return Task.CompletedTask;
        }
    }
}
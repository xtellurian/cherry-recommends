using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Services
{
    public class TokenClaimTenantAuthorizor : ITenantAuthorizationStrategy
    {
        private readonly ILogger<TokenClaimTenantAuthorizor> logger;

        public TokenClaimTenantAuthorizor(ILogger<TokenClaimTenantAuthorizor> logger)
        {
            this.logger = logger;
        }
        public Task Authorize(ClaimsPrincipal principal, Tenant tenant)
        {
            if (principal.Identity.IsAuthenticated)
            {
                if (tenant != null)
                {
                    var scopes = principal.Claims.First(_ => _.Type == "scope").Value.Split(' ');
                    if (scopes.Any(_ => _ == tenant.AccessScope()))
                    {
                        logger.LogInformation($"ClaimsPrincipal {principal.Identity.Name} given access to tenant {tenant.Name}");
                    }
                    else
                    {
                        throw new ForbiddenException("Tenant Access Forbidden", $"ClaimsPrincipal Identity {principal?.Identity?.Name ?? "null"} not allowed to access tenant {tenant.Name}");
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
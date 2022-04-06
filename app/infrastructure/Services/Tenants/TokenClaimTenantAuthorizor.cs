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

        public Task<bool> IsAuthorized(ClaimsPrincipal principal, Tenant tenant)
        {
            if (principal.Identity.IsAuthenticated)
            {
                if (tenant != null)
                {
                    // Type[string]:"permissions"
                    // Value[string]:"tenant:xxxxxxxx"
                    // for when the roles are added automatically when token dialect = access_token_authz
                    var permissions = principal.Claims.Where(_ => _.Type == "permissions");
                    if (permissions.Any(p => p.Value == tenant.AccessScope()))
                    {
                        return Task.FromResult(true);
                    }
                    var scopes = principal.Claims.First(_ => _.Type == "scope").Value.Split(' ');
                    return Task.FromResult(scopes.Any(_ => _ == tenant.AccessScope()));
                }
            }

            // default to false;
            return Task.FromResult(false);
        }

        public async Task Authorize(ClaimsPrincipal principal, Tenant tenant)
        {
            if (tenant == null)
            {
                logger.LogInformation("Allow access - tenant is null");
                return;
            }
            else if (!await this.IsAuthorized(principal, tenant))
            {
                throw new ForbiddenException("Tenant Access Forbidden", $"ClaimsPrincipal Identity {principal?.Identity?.Name ?? "null"} not allowed to access tenant {tenant.Name}");
            }
            else
            {
                logger.LogInformation($"ClaimsPrincipal {principal.Identity.Name} given access to tenant {tenant.Name}");
                return;
            }

        }
    }
}
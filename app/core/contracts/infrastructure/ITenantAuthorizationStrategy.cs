using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantAuthorizationStrategy
    {
        Task Authorize(ClaimsPrincipal principal, Tenant tenant);
        Task<bool> IsAuthorized(ClaimsPrincipal principal, Tenant tenant);
    }
}
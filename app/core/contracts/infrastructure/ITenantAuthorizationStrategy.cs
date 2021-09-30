using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantAuthorizationStrategy
    {
        Task Authorize(ClaimsPrincipal principal, Tenant tenant);
    }
}
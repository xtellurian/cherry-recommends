using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SignalBox.Infrastructure
{
    public static class Auth0Extensions
    {
        public static string Auth0Id(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                return principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            }
            else
            {
                throw new System.ArgumentException("principal must be authenticated to access Auth0Id");
            }
        }
    }
}
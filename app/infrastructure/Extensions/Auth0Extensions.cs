using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using SignalBox.Core.Internal;

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

        public static UserInfo ToCoreRepresentation(this Auth0.ManagementApi.Models.User user)
        {
            return new UserInfo
            {
                UserId = user.UserId,
                Email = user.Email,
                EmailVerified = user.EmailVerified
            };
        }
    }
}
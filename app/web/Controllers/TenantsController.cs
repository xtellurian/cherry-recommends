using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Models;
using SignalBox.Web.Config;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Tenants")]
    public class TenantsController : SignalBoxControllerBase
    {
        private readonly ITenantProvider tenantProvider;
        private readonly ITenantMembershipStore membershipStore;
        private readonly IOptionsMonitor<Hosting> hostingOptions;

        public TenantsController(ITenantProvider tenantProvider,
                                 ITenantMembershipStore membershipStore,
                                 IOptionsMonitor<Hosting> hostingOptions)
        {
            this.tenantProvider = tenantProvider;
            this.membershipStore = membershipStore;
            this.hostingOptions = hostingOptions;
        }

        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tenant))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCurrentTenant()
        {
            if (hostingOptions.CurrentValue.Multitenant)
            {
                var current = tenantProvider.Current();
                if (current != null)
                {
                    return Ok(current);
                }
                else
                {
                    throw new TenantNotFoundException(tenantProvider.RequestedTenantName);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("hosting")]
        [AllowAnonymous]
        public Hosting GetHostingOptions()
        {
            return hostingOptions.CurrentValue;
        }

        [HttpGet("memberships")]
        public async Task<IEnumerable<Tenant>> GetHint()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Auth0Id();
                var memberships = await membershipStore.ReadMemberships(userId);
                if (memberships.Any())
                {
                    return memberships.Select(_ => _.Tenant);
                }
                else
                {
                    throw new MembershipNotFoundException(userId);
                }
            }
            else
            {
                throw new BadRequestException("Unknown User");
            }
        }
    }
}
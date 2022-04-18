using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Internal;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Models;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Tenants")]
    public class TenantsController : SignalBoxControllerBase
    {
        private readonly ITenantProvider tenantProvider;
        private readonly ITenantStore tenantStore;
        private readonly INewTenantQueueStore newTenantQueue;
        private readonly INewTenantMembershipQueueStore newTenantMembersQueue;
        private readonly ITenantMembershipStore membershipStore;
        private readonly IAuth0Service auth0Service;
        private readonly ILogger<TenantsController> logger;
        private readonly IOptionsMonitor<Hosting> hostingOptions;

        public TenantsController(ITenantProvider tenantProvider,
                                 ITenantStore tenantStore,
                                 INewTenantQueueStore newTenantQueue,
                                 ITenantMembershipStore membershipStore,
                                 INewTenantMembershipQueueStore newTenantMembersQueue,
                                 IAuth0Service auth0Service,
                                 ILogger<TenantsController> logger,
                                 IOptionsMonitor<Hosting> hostingOptions)
        {
            this.tenantProvider = tenantProvider;
            this.tenantStore = tenantStore;
            this.newTenantQueue = newTenantQueue;
            this.membershipStore = membershipStore;
            this.newTenantMembersQueue = newTenantMembersQueue;
            this.auth0Service = auth0Service;
            this.logger = logger;
            this.hostingOptions = hostingOptions;
        }

        [HttpGet("Status/{name}")]
        public async Task<StatusDto> GetTenantStatus(string name)
        {
            if (hostingOptions.CurrentValue.Multitenant)
            {
                if (await tenantStore.TenantExists(name))
                {
                    var tenant = await tenantStore.ReadFromName(name);
                    return new StatusDto(tenant.Status);
                }
                else
                {
                    return new StatusDto("Not Exist");
                }
            }
            else
            {
                return new StatusDto("Single Tenant");
            }
        }

        [HttpPost]
        public async Task<StatusDto> CreateTenant(NewTenantDto dto, bool dryRun = false)
        {
            if (hostingOptions.CurrentValue.Multitenant)
            {
                Tenant.ValidateName(dto.Name);
                if (await tenantStore.TenantExists(dto.Name))
                {
                    throw new NameNotAvailableException(dto.Name);
                }

                var creatorId = User.Auth0Id();
                var email = User.Email();
                if (!dryRun)
                {
                    await newTenantQueue.Enqueue(new NewTenantQueueMessage(dto.Name, creatorId, email, dto.TermsOfServiceVersion));
                }

                return new StatusDto("Submitted");
            }
            else
            {
                return new StatusDto("Single Tenant");
            }
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
            }

            return NotFound();
        }

        [HttpGet("current/memberships")]
        public async Task<Paginated<UserInfo>> GetCurrentMemberships()
        {
            var tenant = tenantProvider.Current();
            var memberships = await membershipStore.ReadMemberships(tenant);
            var dtos = new List<UserInfo>();
            foreach (var m in memberships)
            {
                try
                {
                    var info = await auth0Service.GetUserInfo(m.UserId);
                    dtos.Add(info);
                }
                catch (DependencyException dex)
                {
                    // in case Auth0 doesn't recognise the membership
                    logger.LogWarning("Exception message: {message}", dex.Message);
                    dtos.Add(new UserInfo
                    {
                        Email = "Unknown Member",
                        EmailVerified = false,
                        UserId = m.UserId
                    });
                }
            }

            return new Paginated<UserInfo>(dtos, 1, memberships.Count(), 1);
        }

        [HttpPost("current/memberships")]
        public async Task<UserInfo> AddAMembership(CreateTenantMembershipDto dto)
        {
            var tenant = tenantProvider.Current();
            var newUser = await auth0Service.AddUser(new InviteRequest
            {
                Email = dto.Email
            });
            await newTenantMembersQueue.Enqueue(new NewTenantMembershipQueueMessage
            {
                UserId = newUser.UserId,
                TenantName = tenant.Name,
                Email = newUser.Email
            });

            return newUser;
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
                    return new List<Tenant>();
                }
            }
            else
            {
                throw new BadRequestException("Unknown User");
            }
        }
    }
}
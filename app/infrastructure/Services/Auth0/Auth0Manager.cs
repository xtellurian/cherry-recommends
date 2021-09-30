using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Services
{
    public class Auth0Manager : IAuth0Service
    {
        private readonly IApiTokenFactory tokenFactory;
        private readonly Auth0ManagementCredentials credentials;
        private ManagementApiClient client;

        public ManagementApiClient ApiClient => client ?? throw new System.NullReferenceException("ManagementApiClient must be initialized");


        private void SetApiClient(ManagementApiClient value)
        {
            client = value;
        }

        public Auth0Manager(IOptions<Auth0ManagementCredentials> options, IApiTokenFactory tokenFactory)
        {
            this.tokenFactory = tokenFactory;
            this.credentials = options.Value;
            if (string.IsNullOrEmpty(credentials.DefaultAudience))
            {
                throw new System.NullReferenceException("Default Audience cannot be null");
            }
        }

        private async Task Initialize()
        {
            var token = await tokenFactory.GetManagementToken();
            this.SetApiClient(new ManagementApiClient(token, credentials.Domain));
        }

        public async Task AddTenantPermission(string creatorId, Tenant tenant)
        {
            await Initialize();

            var current = await ApiClient.ResourceServers.GetAsync(credentials.DefaultAudience);
            var scopes = current.Scopes.ToList();
            if (!scopes.Any(_ => _.Value == tenant.AccessScope()))
            {
                scopes.Add(new ResourceServerScope
                {
                    Value = tenant.AccessScope(),
                    Description = $"Access permission scope for tenant {tenant.Name} with Id = {tenant.Id}"
                });

                await ApiClient.ResourceServers.UpdateAsync(credentials.DefaultAudience, new ResourceServerUpdateRequest
                {
                    Scopes = scopes
                });
            }

            // var creatorId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            await ApiClient.Users.AssignPermissionsAsync(creatorId, new AssignPermissionsRequest
            {
                Permissions = new List<PermissionIdentity>
                {
                    new PermissionIdentity
                    {
                        Name =tenant.AccessScope(),
                        Identifier = credentials.DefaultAudience // resource identifier
                    }
                }
            });
        }
    }
}
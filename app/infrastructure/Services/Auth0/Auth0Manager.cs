using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Internal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SignalBox.Infrastructure.Services
{
    public class Auth0Manager : IAuth0Service
    {
        private readonly ILogger<Auth0Manager> logger;
        private readonly ITelemetry telemetry;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IApiTokenFactory tokenFactory;
        private readonly Auth0ManagementCredentials credentials;
        private readonly Auth0ReactConfig auth0ReactConfig;
        private ManagementApiClient client;

        public ManagementApiClient ApiClient => client ?? throw new System.NullReferenceException("ManagementApiClient must be initialized");


        private void SetApiClient(ManagementApiClient value)
        {
            client = value;
        }

        public Auth0Manager(IOptions<Auth0ManagementCredentials> options,
                            IOptions<Auth0ReactConfig> auth0ReactConfigOptions,
                            ILogger<Auth0Manager> logger,
                            ITelemetry telemetry,
                            IDateTimeProvider dateTimeProvider,
                            IApiTokenFactory tokenFactory)
        {
            this.logger = logger;
            this.telemetry = telemetry;
            this.dateTimeProvider = dateTimeProvider;
            this.tokenFactory = tokenFactory;
            this.credentials = options.Value;
            this.auth0ReactConfig = auth0ReactConfigOptions.Value;
            if (string.IsNullOrEmpty(credentials.DefaultAudience))
            {
                throw new System.NullReferenceException("Default Audience cannot be null");
            }
        }

        private async Task Initialize()
        {
            var stopwatch = telemetry.NewStopwatch(true);
            var startTime = dateTimeProvider.Now;
            var token = await tokenFactory.GetManagementToken();
            this.SetApiClient(new ManagementApiClient(token.AccessToken, credentials.Domain));
            stopwatch.Stop();
            telemetry.TrackDependency("Auth0", "Initialize", credentials.Domain, startTime, stopwatch.Elapsed, true);
        }

        public async Task<UserInfo> SetMetadata(string userId, UserMetadata metadata)
        {
            await Initialize();
            var user = await ApiClient.Users.UpdateAsync(userId, new UserUpdateRequest
            {
                UserMetadata = metadata,
            });

            return user.ToCoreRepresentation();
        }

        public async Task<UserMetadata> GetMetadata(string userId)
        {
            await Initialize();

            var user = await ApiClient.Users.GetAsync(userId);
            try
            {
                if (user.UserMetadata is JObject jObj)
                {
                    // the underlying object should be a JObject
                    var result = jObj.ToObject<UserMetadata>();
                    result.EnsureInitialised();
                    return result;
                }
            }
            catch (JsonSerializationException jsonEx)
            {
                logger.LogWarning("Unable to deserialize user metadata. UserId = {userId}. {jsonExMessage}", userId, jsonEx.Message);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ConfigurationException("Error getting metadata", ex);
            }

            return new UserMetadata();

        }

        public async Task<UserInfo> GetUserInfo(string userId)
        {
            await Initialize();
            try
            {
                var userInfo = await ApiClient.Users.GetAsync(userId);
                return userInfo.ToCoreRepresentation();
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message);
                throw new DependencyException(ex.Message);
            }
        }

        public async Task CreateRoleForTenant(Tenant tenant)
        {
            await Initialize();

            await GetOrCreateMemberRole(tenant);
        }

        public async Task<string> GetTenantRoleId(Tenant tenant)
        {
            await Initialize();

            var role = await GetOrCreateMemberRole(tenant);
            return role.Id;
        }

        private async Task<Role> GetOrCreateMemberRole(Tenant tenant)
        {
            var stopwatch = telemetry.NewStopwatch(true);
            var startTime = dateTimeProvider.Now;
            await EnsureTenantScopeExists(tenant);

            var roles = await ApiClient.Roles.GetAllAsync(new GetRolesRequest
            {
                NameFilter = tenant.MemberRoleName()
            });
            Role role;
            if (roles.Any())
            {
                logger.LogInformation("Using existing role");
                role = roles.First();
            }
            else
            {
                logger.LogInformation("Creating role for tenant {tenantId}", tenant.Id);
                role = await ApiClient.Roles.CreateAsync(new RoleCreateRequest
                {
                    Name = tenant.MemberRoleName(),
                    Description = $"Gives access to tenant (Id = {tenant.Name})"
                });
            }

            await ApiClient.Roles.AssignPermissionsAsync(role.Id, new AssignPermissionsRequest
            {
                Permissions = new List<PermissionIdentity>
                {
                    new PermissionIdentity
                    {
                        Name = tenant.AccessScope(),
                        Identifier = credentials.DefaultAudience // resource identifier
                    }
                }
            });

            stopwatch.Stop();
            telemetry.TrackDependency("Auth0", "GetOrCreateMemberRole", credentials.Domain, startTime, stopwatch.Elapsed, true);

            return role;
        }

        public async Task<UserInfo> AddUser(InviteRequest invite)
        {
            logger.LogInformation("Inviting {inviteEmail}", invite.Email);
            await Initialize();

            var connections = await ApiClient.Connections.GetAllAsync(new GetConnectionsRequest
            {
                Name = "Username-Password-Authentication"
            }, new Auth0.ManagementApi.Paging.PaginationInfo());

            var connection = connections.First();
            User user;
            var usersExisting = await ApiClient.Users.GetUsersByEmailAsync(invite.Email);
            user = usersExisting.FirstOrDefault();

            user ??= await ApiClient.Users.CreateAsync(new UserCreateRequest
            {
                EmailVerified = false,
                Email = invite.Email,
                Connection = connection.Name,
                Password = System.Guid.NewGuid().ToString()
            });
            var userInfo = new UserInfo
            {
                Email = invite.Email,
                UserId = user.UserId,
            };

            if (user.EmailVerified == false)
            {
                var ticket = await ApiClient.Tickets.CreatePasswordChangeTicketAsync(new PasswordChangeTicketRequest
                {
                    MarkEmailAsVerified = false,
                    UserId = user.UserId,
                    ClientId = auth0ReactConfig.ClientId,

                });

                logger.LogInformation("Ticket URL is {ticketValue}", ticket.Value);
                userInfo.InvitationUrl = ticket.Value;
            }

            return userInfo;
        }

        public async Task AddTenantPermission(string creatorId, Tenant tenant)
        {
            await Initialize();

            var role = await GetOrCreateMemberRole(tenant);

            await ApiClient.Users.AssignRolesAsync(creatorId, new AssignRolesRequest
            {
                Roles = new List<string> { role.Id }.ToArray()
            });
        }

        public async Task AddPermissionToClientGrant(string clientId, Tenant tenant)
        {
            await Initialize();

            var client = await ApiClient.Clients.GetAsync(clientId);
            logger.LogInformation("Got reference to client {clientId}", client.ClientId);
            var grants = await ApiClient.ClientGrants.GetAllAsync(new GetClientGrantsRequest
            {
                Audience = credentials.DefaultAudience,
                ClientId = clientId
            }, new Auth0.ManagementApi.Paging.PaginationInfo());

            if (grants.Any(_ => _.Scope?.Contains(tenant.AccessScope()) == true))
            {
                // scope exists
                logger.LogInformation("Client Grant exists for client {client} to audience {audience} with scope {scope}",
                    client.ClientId, credentials.DefaultAudience, tenant.AccessScope());
            }
            else if (grants.Any())
            {
                // grant exists, so update it.
                var grant = grants.First();
                grant.Scope.Add(tenant.AccessScope());
                await ApiClient.ClientGrants.UpdateAsync(grant.Id, new ClientGrantUpdateRequest
                {
                    Scope = grant.Scope
                });
            }
            else
            {
                await ApiClient.ClientGrants.CreateAsync(new ClientGrantCreateRequest
                {
                    Audience = credentials.DefaultAudience,
                    ClientId = clientId,
                    Scope = new List<string> { tenant.AccessScope() }
                });
            }
        }

        private async Task EnsureTenantScopeExists(Tenant tenant)
        {
            logger.LogInformation("Ensuring tenant scope exists for tenant {tenantName}", tenant.Name);
            var current = await ApiClient.ResourceServers.GetAsync(credentials.DefaultAudience);
            var scopes = current.Scopes.ToList();
            if (!scopes.Any(_ => _.Value == tenant.AccessScope()))
            {
                logger.LogWarning("Creating tenant scope for tenant {tenantName} with Id = {tenantId}", tenant.Name, tenant.Id);
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
            else
            {
                logger.LogInformation("Tenant scope exists.");
            }
        }
    }
}
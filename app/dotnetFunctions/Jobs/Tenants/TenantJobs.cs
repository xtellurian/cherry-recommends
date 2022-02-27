using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Models;
using SignalBox.Infrastructure.Models.Databases;

namespace SignalBox.Functions
{
    public class TenantJobs
    {
        readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly IDatabaseManager dbManager;
        private readonly ITelemetry telemetry;
        private readonly IOptions<Hosting> hostingOptions;
        private readonly IOptions<Auth0M2MClient> m2MClientOptions;
        private readonly ITenantStore tenantStore;
        private readonly ITenantMembershipStore membershipStore;
        private readonly IAuth0Service auth0Service;

        public TenantJobs(IDatabaseManager dbManager,
                          ITelemetry telemetry,
                          IOptions<Hosting> hostingOptions,
                          IOptions<Auth0M2MClient> m2mClientOptions,
                          ITenantStore tenantStore,
                          ITenantMembershipStore membershipStore,
                          IAuth0Service auth0Service)
        {
            this.dbManager = dbManager;
            this.telemetry = telemetry;
            this.hostingOptions = hostingOptions;
            m2MClientOptions = m2mClientOptions;
            this.tenantStore = tenantStore;
            this.membershipStore = membershipStore;
            this.auth0Service = auth0Service;
        }

        [Function("CreateTenant_FromQueue")]
        public async Task TriggerCreateTenant_FromQueue(
            [QueueTrigger(Core.Constants.AzureQueueNames.NewTenants)] NewTenantQueueMessage message,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateTenant_QueueTrigger");
            if (hostingOptions.Value.Multitenant)
            {
                var info = new CreateTenantModel
                {
                    Name = message.Name,
                    CreatorId = message.CreatorId,
                    CreatorEmail = message.CreatorEmail
                };

                await CreateTenant(info, logger);
            }
            else
            {
                throw new BadRequestException("Cannot create a tenant in a non-multitenant environment");
            }
        }

        [Function("CreateTenant")]
        public async Task<Tenant> TriggerCreateTenant([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "Tenants")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateTenant");
            if (hostingOptions.Value.Multitenant)
            {
                var info = await JsonSerializer.DeserializeAsync<CreateTenantModel>(req.Body, serializerOptions);
                return await CreateTenant(info, logger);
            }
            else
            {
                throw new BadRequestException("Cannot create a tenant in a non-multitenant environment");
            }
        }

        [Function("ListTenants")]
        public async Task<IEnumerable<Tenant>> ListTenants([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "Tenants")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ListTenants");
            if (hostingOptions.Value.Multitenant)
            {

                return await tenantStore.List();
            }
            else
            {
                throw new BadRequestException("Cannot list tenants in a non-multitenant environment");
            }
        }

        [Function("ListMemberships")]
        public async Task<IEnumerable<TenantMembership>> ListMemberships([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "Tenants/{tenantName}/Members")]
            HttpRequestData req,
            FunctionContext executionContext,
            string tenantName)
        {
            var logger = executionContext.GetLogger("ListMemberships");
            if (hostingOptions.Value.Multitenant)
            {

                if (await tenantStore.TenantExists(tenantName))
                {
                    var tenant = await tenantStore.ReadFromName(tenantName);
                    var memberships = await membershipStore.ReadMemberships(tenant);
                    return memberships;
                }
                else
                {
                    throw new BadRequestException($"{tenantName} tenant doesnt exist");
                }
            }
            else
            {
                throw new BadRequestException("Cannot list memberships in a non-multitenant environment");
            }
        }

        [Function("GetRoleIds")]
        public async Task<IEnumerable<string>> GetTenantRoleId([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "Tenants/{tenantName}/RoleId")]
            HttpRequestData req, FunctionContext executionContext, string tenantName)
        {
            var result = new List<string>();
            if (tenantName == "*")
            {
                foreach (var tenant in await tenantStore.List())
                {
                    var id = await auth0Service.GetTenantRoleId(tenant);
                    result.Add(id);
                }
            }
            else
            {
                var tenant = await tenantStore.ReadFromName(tenantName);
                var id = await auth0Service.GetTenantRoleId(tenant);
                result.Add(id);
            }

            return result;
        }


        [Function("CreateTenantMembership_FromQueue")]
        public async Task TriggerCreateTenantMembership_FromQueue(
           [QueueTrigger(SignalBox.Core.Constants.AzureQueueNames.NewTenantMemberships)] NewTenantMembershipQueueMessage message,
           FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("TriggerCreateTenantMembership_FromQueue");
            // create a 
            await AddTenantMember(new AddMember(message.UserId, message.Email), message.TenantName, logger);
        }

        [Function("AddMember")]
        public async Task<Tenant> TriggerAddMember([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "Tenants/{tenantName}/Members")] HttpRequestData req, string tenantName,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AddMember");
            var addMember = await JsonSerializer.DeserializeAsync<AddMember>(req.Body, serializerOptions);
            return await AddTenantMember(addMember, tenantName, logger);
        }

        [Function("AssignRoles")]
        public async Task<IEnumerable<TenantMembership>> AssignRoles([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "Tenants/{tenantName}/Members/{userId}")] HttpRequestData req, string tenantName, string userId,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AssignRoles");
            var memberships = new List<TenantMembership>();
            if (tenantName == "*")
            {
                foreach (var tenant in await tenantStore.List())
                {
                    memberships.AddRange(await AssignMemberTenantRole(tenant, userId));
                }
            }
            else
            {
                var tenant = await tenantStore.ReadFromName(tenantName);
                memberships.AddRange(await AssignMemberTenantRole(tenant, userId));
            }
            logger.LogInformation("Updated {n} membership roles", memberships.Count);
            return memberships;
        }

        private async Task<IEnumerable<TenantMembership>> AssignMemberTenantRole(Tenant tenant, string userId)
        {
            try
            {
                var memberships = new List<TenantMembership>();
                if (userId == "*")
                {
                    // the get all users
                    foreach (var member in await membershipStore.ReadMemberships(tenant))
                    {
                        await auth0Service.AddTenantPermission(member.UserId, tenant);
                        memberships.Add(member);
                    }
                }
                else
                {
                    var membershipsForUser = await membershipStore.ReadMemberships(userId);
                    var membership = membershipsForUser.First(_ => _.TenantId == tenant.Id);
                    memberships.Add(membership);
                    await auth0Service.AddTenantPermission(userId, tenant);
                    memberships.Add(membership);
                }
                return memberships;
            }
            catch (System.Exception ex)
            {
                throw new DependencyException($"AssignMemberTenantRole failed for {userId}", ex);
            }
        }

        private async Task<Tenant> AddTenantMember(AddMember addMember, string tenantName, ILogger logger)
        {
            if (hostingOptions.Value.Multitenant)
            {

                if (await tenantStore.TenantExists(tenantName))
                {
                    var tenant = await tenantStore.ReadFromName(tenantName);
                    await AddUserToTenant(addMember.UserId, tenant, logger);
                    await tenantStore.SaveChanges();
                    return tenant;
                }
                else
                {
                    throw new BadRequestException($"{tenantName} doesnt exist");
                }
            }
            else
            {
                throw new BadRequestException("Cannot add members in a non-multitenant environment");
            }
        }

        [Function("MigrateTenant")]
        public async Task<IEnumerable<MigrationResult>> TriggerMigrate([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "Tenants/{tenantName}/migrations")] HttpRequestData req,
            FunctionContext executionContext, string tenantName)
        {
            var logger = executionContext.GetLogger("MigrateTenant");

            if (hostingOptions.Value.Multitenant)
            {
                if (tenantName == "*")
                {
                    logger.LogInformation("Migrating all tenants");
                    // then migrate all tenants
                    var results = new List<MigrationResult>();
                    foreach (var t in await tenantStore.List())
                    {
                        try
                        {
                            // get the tenant role ID - should refresh the scopes and roles
                            logger.LogInformation($"Migrating tenant {t.Name} with database {t.DatabaseName}");
                            var result = await dbManager.MigrateDatabase(t, _ => _.FixSqliteConnectionString());

                            result.Auth0RoleId = await auth0Service.GetTenantRoleId(t);
                            results.Add(result);
                        }
                        catch (System.Exception ex)
                        {
                            telemetry.TrackException(ex);
                            logger.LogCritical("Error migrating tenant {Name} with database {DatabaseName}. Message: ", t.Name, t.DatabaseName, ex.Message);
                        }
                    }
                    return results;
                }
                else if (await tenantStore.TenantExists(tenantName))
                {
                    var tenant = await tenantStore.ReadFromName(tenantName);
                    var result = await dbManager.MigrateDatabase(tenant, _ => _.FixSqliteConnectionString());
                    return new List<MigrationResult> { result };
                }
                else
                {
                    throw new BadRequestException($"{tenantName} doesnt exist");
                }
            }
            else
            {
                throw new BadRequestException("Cannot create a tenant in a non-multitenant environment");
            }
        }

        [Function("ListMigrations")]
        public async Task<IEnumerable<MigrationInfo>> ListMigrations([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "Tenants/{tenantName}/migrations")] HttpRequestData req,
            FunctionContext executionContext, string tenantName)
        {
            var logger = executionContext.GetLogger("MigrateTenant");

            if (hostingOptions.Value.Multitenant)
            {
                if (await tenantStore.TenantExists(tenantName))
                {
                    var tenant = await tenantStore.ReadFromName(tenantName);
                    return await dbManager.ListMigrations(tenant, _ => _.FixSqliteConnectionString());
                }
                else
                {
                    throw new BadRequestException($"Tenant {tenantName} doesn't exist");
                }
            }
            else
            {
                throw new BadRequestException("Can't list migrations in a single tenant environment");
            }

        }

        private async Task<Tenant> CreateTenant(CreateTenantModel info, ILogger logger)
        {
            var tenantCreateStopwatch = telemetry.NewStopwatch(true);

            info.Validate();
            if (await tenantStore.TenantExists(info.Name))
            {
                throw new BadRequestException($"Tenant {info.Name} already exists");
            }

            var tenant = new Tenant(info.Name, info.Name + '-' + System.Guid.NewGuid().ToString().ToLowerInvariant());
            tenant = await tenantStore.Create(tenant);
            var terms = new TenantTermsOfServiceAcceptance(tenant, info.TermsOfServiceVersion, info.CreatorId);
            await tenantStore.SaveChanges();

            await dbManager.CreateDatabase(tenant, _ => _.FixSqliteConnectionString());
            tenant.Status = Tenant.Status_Database_Created;
            await tenantStore.SaveChanges();
            // create role for accessing tenant
            await CreateRoleForTenant(tenant);

            await AddUserToTenant(info.CreatorId, tenant, logger, info.CreatorEmail);
            tenant.Status = Tenant.Status_Created;
            await tenantStore.SaveChanges();

            // give the M2M account access to the tenant
            await auth0Service.AddPermissionToClientGrant(m2MClientOptions.Value.ClientId, tenant);

            tenantCreateStopwatch.Stop();
            telemetry.TrackMetric("CreateTenant.TimeElapsed.Seconds", tenantCreateStopwatch.Elapsed.TotalSeconds);
            return tenant;
        }

        private async Task CreateRoleForTenant(Tenant tenant)
        {
            await auth0Service.CreateRoleForTenant(tenant);
        }

        private async Task AddUserToTenant(string userId, Tenant tenant, ILogger logger, string email = null)
        {
            await auth0Service.AddTenantPermission(userId, tenant);
            if (await membershipStore.IsMember(tenant, userId))
            {
                logger.LogWarning($"{userId} is already a member of tenant {tenant.Name}");
            }
            else
            {
                logger.LogInformation($"Adding {userId} to tenant {tenant.Name}");
                var membership = await membershipStore.Create(new TenantMembership(tenant, userId, email));
                logger.LogInformation($"Created Membership {membership.Id}");
            }
        }
    }
}
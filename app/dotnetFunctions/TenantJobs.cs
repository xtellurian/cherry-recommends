using System.Collections.Generic;
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
    public class CreateNewSqlDatabase
    {
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly IDatabaseManager dbManager;
        private readonly IOptions<Hosting> hostingOptions;
        private readonly ITenantStore tenantStore;
        private readonly ITenantMembershipStore membershipStore;
        private readonly IAuth0Service auth0Service;

        public CreateNewSqlDatabase(IDatabaseManager dbManager,
                                    IOptions<Hosting> hostingOptions,
                                    ITenantStore tenantStore,
                                    ITenantMembershipStore membershipStore,
                                    IAuth0Service auth0Service)
        {
            this.dbManager = dbManager;
            this.hostingOptions = hostingOptions;
            this.tenantStore = tenantStore;
            this.membershipStore = membershipStore;
            this.auth0Service = auth0Service;
        }

        [Function("CreateTenant_FromQueue")]
        public async Task TriggerCreateTenant_FromQueue(
            [QueueTrigger(SignalBox.Core.Constants.AzureQueueNames.NewTenants)] NewTenantQueueMessage message,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateTenant_QueueTrigger");
            if (hostingOptions.Value.Multitenant)
            {
                var info = new CreateTenantModel
                {
                    Name = message.Name,
                    CreatorId = message.CreatorId
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
            Route = "Tenants/{tenantName:alpha}/Members")]
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

        [Function("AddMember")]
        public async Task<Tenant> TriggerAddMember([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "Tenants/{tenantName:alpha}/Members")] HttpRequestData req, string tenantName,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AddMember");
            if (hostingOptions.Value.Multitenant)
            {
                var info = await JsonSerializer.DeserializeAsync<AddMember>(req.Body, serializerOptions);

                if (await tenantStore.TenantExists(tenantName))
                {
                    var tenant = await tenantStore.ReadFromName(tenantName);
                    await AddUserToTenant(info.UserId, tenant, logger);
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
                            logger.LogInformation($"Migrating tenant {t.Name} with database {t.DatabaseName}");
                            var result = await dbManager.MigrateDatabase(t, _ => _.FixSqliteConnectionString());
                            results.Add(result);
                        }
                        catch (System.Exception ex)
                        {
                            logger.LogCritical($"Error migrating tenant {t.Name} with database {t.DatabaseName}");
                            logger.LogError(ex.Message);
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
            await AddUserToTenant(info.CreatorId, tenant, logger);
            tenant.Status = Tenant.Status_Created;
            await tenantStore.SaveChanges();
            return tenant;
        }

        private async Task AddUserToTenant(string userId, Tenant tenant, ILogger logger)
        {
            await auth0Service.AddTenantPermission(userId, tenant);
            if (await membershipStore.IsMember(tenant, userId))
            {
                logger.LogWarning($"{userId} is already a member of tenant {tenant.Name}");
            }
            else
            {
                logger.LogInformation($"Adding {userId} to tenant {tenant.Name}");
                var membership = await membershipStore.Create(new TenantMembership(tenant, userId));
            }
        }
    }
}
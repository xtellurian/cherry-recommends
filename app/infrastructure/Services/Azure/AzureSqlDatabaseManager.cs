using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Azure
{
    public class AzureSqlDatabaseManager : IDatabaseManager
    {
        private const string migrationAssembly = "sqlserver";
        private readonly SignalBoxAzureEnvironment azureEnv;
        private readonly IAzure azure;
        private readonly ILogger<AzureSqlDatabaseManager> logger;

        public AzureSqlDatabaseManager(IOptions<SignalBoxAzureEnvironment> azureEnv, ILogger<AzureSqlDatabaseManager> logger)
        {
            this.azureEnv = azureEnv.Value;
            this.logger = logger;
            this.azure = Connect();
        }

        private IAzure Connect()
        {
            try
            {
                // var creds = AzureCliCredentials.Create();
                var defaultCredential = new DefaultAzureCredential(includeInteractiveCredentials: true);
                var defaultToken = defaultCredential.GetToken(new TokenRequestContext(new[] { "https://management.azure.com/.default" })).Token;
                logger.LogInformation("Loaded a default token");
                var defaultTokenCredentials = new TokenCredentials(defaultToken);
                var azureCredentials = new AzureCredentials(defaultTokenCredentials, defaultTokenCredentials, null, AzureEnvironment.AzureGlobalCloud);
                var azure = Microsoft.Azure.Management.Fluent.Azure.Configure().Authenticate(azureCredentials).WithSubscription(azureEnv.SubscriptionId);
                logger.LogInformation("Created an Azure client");
                return azure;
            }
            catch (Exception ex)
            {
                logger.LogCritical("Failed to initialize IAzure");
                logger.LogCritical(ex.Message);
                throw;
            }
        }

        public async Task CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new System.NullReferenceException("Database name cannot be null or empty");
            }
            logger.LogInformation($"Creating SQL Database {tenant.DatabaseName} for tenant {tenant.Name}");
            var server = await azure.SqlServers.GetByIdAsync(this.azureEnv.SqlServerAzureId);
            if (server == null)
            {
                throw new System.NullReferenceException("Sql Server not found");
            }
            logger.LogInformation($"Using Azure Sql Server: {server.Name}");

            var database = await server.Databases
                            .Define(tenant.DatabaseName)
                            .WithExistingElasticPool(azureEnv.ElasticPoolName)
                            .WithTag("tenant", tenant.Name)
                            .WithTag("tenantId", tenant.Id.ToString())
                            .CreateAsync();

            await MigrateDatabase(server, database, manipulateConnectionString);
            logger.LogInformation("Completed Create Database operation");
        }

        // private method called during create and during migrate
        private async Task MigrateDatabase(ISqlServer server, ISqlDatabase database, Func<string, string> manipulateConnectionString)
        {
            var options = new DbContextOptionsBuilder<SignalBoxDbContext>();
            var connectionString = SqlServerConnectionStringFactory.GenerateAzureSqlConnectionString(
                server.Name, database.Name, azureEnv.SqlServerUserName, azureEnv.SqlServerPassword);
            if (manipulateConnectionString != null)
            {
                connectionString = manipulateConnectionString(connectionString);
            }
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180));
            var context = new SignalBoxDbContext(options.Options);
            logger.LogInformation("Migrating database...");
            await context.Database.MigrateAsync();
            logger.LogInformation($"Migrated database {database.Name}");
        }

        public async Task MigrateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new NullReferenceException("Database name cannot be null or empty");
            }
            logger.LogInformation($"Migrating SQL Database {tenant.DatabaseName} for tenant {tenant.Name}");
            var server = await azure.SqlServers.GetByIdAsync(this.azureEnv.SqlServerAzureId);
            if (server == null)
            {
                throw new System.NullReferenceException("Sql Server not found");
            }
            logger.LogInformation($"Using Azure Sql Server: {server.Name}");

            var database = await server.Databases.GetAsync(tenant.DatabaseName);
            if (database == null)
            {
                throw new NullReferenceException($"Database {tenant.DatabaseName} not found in server {server.Name}");
            }
            await this.MigrateDatabase(server, database, manipulateConnectionString);
        }
    }
}
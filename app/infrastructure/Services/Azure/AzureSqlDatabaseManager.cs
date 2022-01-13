using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using SignalBox.Core;
using SignalBox.Infrastructure.Models.Databases;

namespace SignalBox.Infrastructure.Azure
{
    public class AzureSqlDatabaseManager : IDatabaseManager
    {
        private const string migrationAssembly = "sqlserver";
        private readonly SignalBoxAzureEnvironment azureEnv;
        private readonly IAzure azure;
        private readonly ITelemetry telemetry;
        private readonly IDateTimeProvider dtProvider;
        private readonly ILogger<AzureSqlDatabaseManager> logger;

        public AzureSqlDatabaseManager(IOptions<SignalBoxAzureEnvironment> azureEnv,
                                       ITelemetry telemetry,
                                       IDateTimeProvider dtProvider,
                                       ILogger<AzureSqlDatabaseManager> logger)
        {
            this.azureEnv = azureEnv.Value;
            this.telemetry = telemetry;
            this.dtProvider = dtProvider;
            this.logger = logger;
            this.azure = Connect();
        }

        private IAzure Connect()
        {
            var stopwatch = telemetry.NewStopwatch(true);
            var startTime = dtProvider.Now;
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
                stopwatch.Stop();
                telemetry.TrackDependency("Azure", azure.SubscriptionId, "Connect", startTime, stopwatch.Elapsed, true);
                return azure;
            }
            catch (Exception ex)
            {
                telemetry.TrackDependency("Azure", azure.SubscriptionId, "Connect", startTime, stopwatch.Elapsed, false);
                logger.LogCritical("Failed to initialize IAzure");
                logger.LogCritical(ex.Message);
                throw;
            }
        }

        public async Task<MigrationResult> CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            var stopwatch = telemetry.NewStopwatch(true);
            var startTime = dtProvider.Now;
            var result = new MigrationResult(tenant);
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new NullReferenceException("Database name cannot be null or empty");
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

            await MigrateDatabase(server, database, result, manipulateConnectionString);
            stopwatch.Stop();
            logger.LogInformation("Completed Create Database operation");

            telemetry.TrackDependency("Azure", "CreateDatabase", azure.SubscriptionId, startTime, stopwatch.Elapsed, true);
            return result;
        }

        // private method called during create and during migrate
        private async Task MigrateDatabase(ISqlServer server, ISqlDatabase database, MigrationResult result, Func<string, string> manipulateConnectionString)
        {
            var stopwatch = telemetry.NewStopwatch(true);
            var startTime = dtProvider.Now;
            var options = CreateDbContextOptions(server, database, manipulateConnectionString);
            using (var context = new SignalBoxDbContext(options.Options))
            {
                var applied = await context.Database.GetAppliedMigrationsAsync();
                result.AddMigrations(true, applied);

                var pending = await context.Database.GetPendingMigrationsAsync();
                result.AddMigrations(false, pending);

                logger.LogInformation($"Migrating database. There are {pending.Count()} pending migrations and {applied.Count()} applied.");
                await context.Database.MigrateAsync();
            }
            logger.LogInformation($"Migrated database {database.Name}");

            stopwatch.Stop();
            telemetry.TrackDependency("AzureSql", "MigrateDatabase", "SignalBoxDbContext, db=" + database.Name, startTime, stopwatch.Elapsed, true);
        }

        private DbContextOptionsBuilder<SignalBoxDbContext> CreateDbContextOptions(ISqlServer server, ISqlDatabase database, Func<string, string> manipulateConnectionString)
        {
            var options = new DbContextOptionsBuilder<SignalBoxDbContext>();
            var connectionString = SqlServerConnectionStringFactory.GenerateAzureSqlConnectionString(
                server.Name, database.Name, azureEnv.SqlServerUserName, azureEnv.SqlServerPassword);
            if (manipulateConnectionString != null)
            {
                connectionString = manipulateConnectionString(connectionString);
            }
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180));
            return options;
        }

        public async Task<MigrationResult> MigrateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new NullReferenceException("Database name cannot be null or empty");
            }
            var result = new MigrationResult(tenant);
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
            await MigrateDatabase(server, database, result, manipulateConnectionString);
            return result;
        }

        public async Task<IEnumerable<MigrationInfo>> ListMigrations(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new NullReferenceException("Database name cannot be null or empty");
            }

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

            var migrations = new List<MigrationInfo>();
            var options = CreateDbContextOptions(server, database, manipulateConnectionString);
            using (var context = new SignalBoxDbContext(options.Options))
            {
                var applied = await context.Database.GetAppliedMigrationsAsync();
                migrations.AddRange(applied.Select(_ => new MigrationInfo(_, true)));

                var pending = await context.Database.GetPendingMigrationsAsync();
                migrations.AddRange(pending.Select(_ => new MigrationInfo(_, false)));

            }
            return migrations;
        }
    }
}
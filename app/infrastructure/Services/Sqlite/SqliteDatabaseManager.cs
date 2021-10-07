using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models.Databases;

namespace SignalBox.Infrastructure.Sqlite
{
    public class SqliteDatabaseManager : IDatabaseManager
    {
        private SqliteDatabasesConfig config;
        private const string migrationAssembly = "sqlite";
        private readonly ILogger<SqliteDatabaseManager> logger;

        public SqliteDatabaseManager(ILogger<SqliteDatabaseManager> logger, IOptions<SqliteDatabasesConfig> options)
        {
            this.config = options.Value;
            if (config.Directory == null)
            {
                throw new System.NullReferenceException("Sqlite Database config directory cannot be null");
            }

            this.logger = logger;
        }

        public async Task<MigrationResult> CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            var result = new MigrationResult(tenant);
            logger.LogInformation($"Creating database {tenant.DatabaseName} in directory {config.Directory}");
            try
            {
                var options = ConstructDbContextOptions(tenant, manipulateConnectionString);
                using (var context = new SignalBoxDbContext(options.Options))
                {
                    var pending = await context.Database.GetPendingMigrationsAsync();
                    result.AddMigrations(true, pending);
                    await context.Database.MigrateAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to create or migrate database {tenant.DatabaseName} for tenant {tenant.Name}");
                logger.LogError(ex.Message);
                throw;
            }
        }



        public async Task<MigrationResult> MigrateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            // is the same as create when using the Sqlite provider
            return await this.CreateDatabase(tenant, manipulateConnectionString);
        }

        public async Task<IEnumerable<MigrationInfo>> ListMigrations(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            logger.LogInformation($"Listing migrations for database {tenant.DatabaseName} in directory {config.Directory}");
            try
            {
                var result = new List<MigrationInfo>();
                var options = ConstructDbContextOptions(tenant, manipulateConnectionString);
                using (var context = new SignalBoxDbContext(options.Options))
                {
                    var applied = await context.Database.GetAppliedMigrationsAsync();
                    result.AddRange(applied.Select(_ => new MigrationInfo(_, true)));
                    var pending = await context.Database.GetPendingMigrationsAsync();
                    result.AddRange(pending.Select(_ => new MigrationInfo(_, false)));

                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to create or migrate database {tenant.DatabaseName} for tenant {tenant.Name}");
                logger.LogError(ex.Message);
                throw;
            }
        }

        private DbContextOptionsBuilder<SignalBoxDbContext> ConstructDbContextOptions(Tenant tenant, Func<string, string> manipulateConnectionString)
        {
            var options = new DbContextOptionsBuilder<SignalBoxDbContext>();
            var connectionString = SqlLiteConnectionStringFactory.GenerateConnectionString(config.AppDir, config.Directory, tenant.DatabaseName);
            if (manipulateConnectionString != null)
            {
                connectionString = manipulateConnectionString(connectionString);
            }
            options.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180));
            return options;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models.Databases;

namespace SignalBox.Infrastructure.Azure
{
    public class LocalSqlDatabaseManager : IDatabaseManager
    {
        private const string migrationAssembly = "sqlserver";
        private readonly LocalSqlCredentials localSql;
        private readonly ILogger<LocalSqlDatabaseManager> logger;

        public LocalSqlDatabaseManager(IOptions<LocalSqlCredentials> localSql,
                                       ILogger<LocalSqlDatabaseManager> logger)
        {
            this.localSql = localSql.Value;
            this.logger = logger;
        }

        public async Task<MigrationResult> CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new NullReferenceException("Database name cannot be null or empty");
            }
            var result = new MigrationResult(tenant);
            await MigrateDatabase(tenant.DatabaseName, result);
            return result;
        }

        // private method called during create and during migrate
        private async Task MigrateDatabase(string databaseName, MigrationResult result)
        {
            var options = CreateDbContextOptions(databaseName);
            using (var context = new SignalBoxDbContext(options.Options))
            {
                var pending = await context.Database.GetPendingMigrationsAsync();
                result.AddPendingMigrations(true, pending);
                await context.Database.MigrateAsync();
            }
            logger.LogInformation("Migrated database {databaseName}", databaseName);
        }

        private DbContextOptionsBuilder<SignalBoxDbContext> CreateDbContextOptions(string databaseName)
        {
            var options = new DbContextOptionsBuilder<SignalBoxDbContext>();
            var connectionString = SqlServerConnectionStringFactory.GenerateLocalSqlConnectionString(
                 databaseName, localSql.SqlServerUserName, localSql.SqlServerPassword);
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
            logger.LogInformation("Migrating SQL Database {dbName} for tenant {tenant=Name}", tenant.DatabaseName, tenant.Name);

            await MigrateDatabase(tenant.DatabaseName, result);
            return result;
        }

        public async Task<IEnumerable<MigrationInfo>> ListMigrations(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            if (string.IsNullOrEmpty(tenant.DatabaseName))
            {
                throw new NullReferenceException("Database name cannot be null or empty");
            }

            logger.LogInformation("Using Local Sql Server");

            var migrations = new List<MigrationInfo>();
            var options = CreateDbContextOptions(tenant.DatabaseName);
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
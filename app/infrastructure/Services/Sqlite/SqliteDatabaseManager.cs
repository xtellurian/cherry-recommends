using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;

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

        public async Task CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            logger.LogInformation($"Creating database {tenant.DatabaseName} in directory {config.Directory}");
            try
            {
                var options = new DbContextOptionsBuilder<SignalBoxDbContext>();
                var connectionString = SqlLiteConnectionStringFactory.GenerateConnectionString(config.AppDir, config.Directory, tenant.DatabaseName);
                if (manipulateConnectionString != null)
                {
                    connectionString = manipulateConnectionString(connectionString);
                }
                options.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180));
                using (var context = new SignalBoxDbContext(options.Options))
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to create or migrate database {tenant.DatabaseName} for tenant {tenant.Name}");
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task MigrateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null)
        {
            // is the same as create when using the Sqlite provider
            await this.CreateDatabase(tenant, manipulateConnectionString);
        }
    }
}
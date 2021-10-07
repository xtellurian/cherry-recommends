using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure.Models.Databases;

namespace SignalBox.Infrastructure
{
    public interface IDatabaseManager
    {
        Task<MigrationResult> CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null);
        Task<MigrationResult> MigrateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null);
        Task<IEnumerable<MigrationInfo>> ListMigrations(Tenant tenant, Func<string, string> manipulateConnectionString = null);
    }
}
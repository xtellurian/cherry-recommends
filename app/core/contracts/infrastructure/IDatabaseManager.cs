using System;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IDatabaseManager
    {
        Task CreateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null);
        Task MigrateDatabase(Tenant tenant, Func<string, string> manipulateConnectionString = null);
    }
}
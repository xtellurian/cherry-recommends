using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Models.Databases
{
    public class MigrationResult
    {
        public Tenant Tenant { get; set; }
        public List<MigrationInfo> PendingMigrations { get; set; } = new List<MigrationInfo>();
        public string Auth0RoleId { get; set; }
        public string ExceptionMessage { get; set; }

        protected MigrationResult() { }
        public MigrationResult(Tenant tenant)
        {
            Tenant = tenant;
        }

        public MigrationResult(Tenant tenant, System.Exception exception)
        {
            Tenant = tenant;
            ExceptionMessage = exception.Message;
        }

        public void AddPendingMigrations(bool willBeApplied, IEnumerable<string> names)
        {
            PendingMigrations.AddRange(names.Select(_ => new MigrationInfo(_, willBeApplied)));
        }
    }
}
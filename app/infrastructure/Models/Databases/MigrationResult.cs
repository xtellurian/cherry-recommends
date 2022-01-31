using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Models.Databases
{
    public class MigrationResult
    {
        public Tenant Tenant { get; set; }
        public List<MigrationInfo> Migrations { get; set; }
        public string Auth0RoleId { get; set; }

        protected MigrationResult() { }
        public MigrationResult(Tenant tenant)
        {
            Tenant = tenant;
            Migrations = new List<MigrationInfo>();
        }

        public void AddMigration(bool isApplied, string name)
        {
            Migrations.Add(new MigrationInfo(name, isApplied));
        }

        public void AddMigrations(bool isApplied, IEnumerable<string> names)
        {
            Migrations.AddRange(names.Select(_ => new MigrationInfo(_, isApplied)));
        }
    }
}
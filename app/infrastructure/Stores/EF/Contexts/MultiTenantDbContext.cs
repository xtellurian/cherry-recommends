
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Accounts;
using SignalBox.Infrastructure.EntityFramework;

namespace SignalBox.Infrastructure
{
    public class MultiTenantDbContext : DbContextBase
    {
        public MultiTenantDbContext(DbContextOptions<MultiTenantDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configure the multitenant types
            new TenantTypeConfiguration().Configure(modelBuilder.Entity<Tenant>());
            new BillingAccountTypeConfiguration().Configure(modelBuilder.Entity<BillingAccount>());
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantMembership> Memberships { get; set; }
    }
}

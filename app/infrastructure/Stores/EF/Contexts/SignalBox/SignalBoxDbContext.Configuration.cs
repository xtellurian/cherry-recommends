using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalBox.Core;
using SignalBox.Infrastructure.EntityFramework;

namespace SignalBox.Infrastructure
{
    public partial class SignalBoxDbContext : DbContextBase
    {
        public SignalBoxDbContext(DbContextOptions<SignalBoxDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackedUserTypeConfiguration).Assembly,
                _ => _.Namespace.StartsWith(typeof(TrackedUserTypeConfiguration).Namespace));
            FixSqlLite(modelBuilder);
            modelBuilder.Entity<Tenant>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<TenantMembership>().Metadata.SetIsTableExcludedFromMigrations(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private void FixSqlLite(ModelBuilder modelBuilder)
        {
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                                || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        modelBuilder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            GenerateLastUpdated();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void GenerateLastUpdated()
        {
            // ChangeTracker.DetectChanges(); // do we need this?
            var now = DateTimeOffset.UtcNow;

            foreach (var item in ChangeTracker.Entries<Entity>().Where(e => e.State == EntityState.Modified))
            {
                item.Property(nameof(Entity.LastUpdated)).CurrentValue = now;
            }
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalBox.Core;
using SignalBox.Infrastructure.EntityFramework;

namespace SignalBox.Infrastructure
{
    public class SignalBoxDbContext : DbContext
    {
        public SignalBoxDbContext(DbContextOptions<SignalBoxDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderTypeConfiguration).Assembly,
                _ => _.Namespace.StartsWith(typeof(OrderTypeConfiguration).Namespace));

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

        // core stuff
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferRecommendation> Recommendations { get; set; }
        public DbSet<PresentationOutcome> PresentationOutcomes { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sku> Skus { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<TrackedUser> TrackedUsers { get; set; }
        public DbSet<TrackedUserEvent> TrackedUserEvents { get; set; }
        public DbSet<Touchpoint> Touchpoints { get; set; }
        public DbSet<TrackedUserTouchpoint> TrackedUserTouchpoints { get; set; }

        // system stuff
        public DbSet<HashedApiKey> ApiKeys { get; set; }
        public DbSet<ModelRegistration> ModelRegistrations { get; set; }
        public DbSet<IntegratedSystem> IntegratedSystems { get; set; }
        public DbSet<WebhookReceiver> WebhookReceivers { get; set; }
        public DbSet<TrackedUserSystemMap> TrackUserSystemMaps { get; set; }

    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Infrastructure.EntityFramework;

namespace SignalBox.Infrastructure
{
    public class SignalBoxDbContext : DbContextBase
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

        // core stuff
        public DbSet<Core.Environment> Environments { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<RecommendableItem> RecommendableItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<TrackedUser> TrackedUsers { get; set; }
        public DbSet<TrackedUserEvent> TrackedUserEvents { get; set; }
        public DbSet<TrackedUserAction> TrackedUserActions { get; set; }
        public DbSet<RewardSelector> RewardSelectors { get; set; }
        public DbSet<Touchpoint> Touchpoints { get; set; }
        public DbSet<TrackedUserTouchpoint> TrackedUserTouchpoints { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        // features
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureGenerator> FeatureGenerators { get; set; }
        public DbSet<HistoricTrackedUserFeature> HistoricTrackedUserFeatures { get; set; }
        public DbSet<LatestFeatureVersion> LatestFeatureVersions { get; set; } // SQL view

        // recommenders
        public DbSet<RecommenderEntityBase> Recommenders { get; set; }
        public DbSet<ProductRecommender> ProductRecommenders { get; set; }
        public DbSet<ParameterSetRecommender> ParameterSetRecommenders { get; set; }
        public DbSet<ItemsRecommender> ItemsRecommenders { get; set; }

        // recommendations
        public DbSet<RecommendationCorrelator> RecommendationCorrelators { get; set; }
        public DbSet<ParameterSetRecommendation> ParameterSetRecommendations { get; set; }
        public DbSet<ItemsRecommendation> ItemsRecommendations { get; set; }
        public DbSet<ProductRecommendation> ProductRecommendations { get; set; }

        // system stuff
        public DbSet<HashedApiKey> ApiKeys { get; set; }
        public DbSet<ModelRegistration> ModelRegistrations { get; set; }
        public DbSet<IntegratedSystem> IntegratedSystems { get; set; }
        public DbSet<WebhookReceiver> WebhookReceivers { get; set; }
        public DbSet<TrackedUserSystemMap> TrackUserSystemMaps { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
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
        }

        // core stuff
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferRecommendation> Recommendations { get; set; }
        public DbSet<PresentationOutcome> PresentationOutcomes { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<TrackedUser> TrackedUsers { get; set; }
        public DbSet<TrackedUserEvent> TrackedUserEvents { get; set; }

        // system stuff
        public DbSet<HashedApiKey> ApiKeys { get; set; }
        public DbSet<ModelRegistration> ModelRegistrations { get; set; }
        public DbSet<IntegratedSystem> IntegratedSystems { get; set; }
        public DbSet<TrackedUserSystemMap> TrackUserSystemMaps { get; set; }

    }
}

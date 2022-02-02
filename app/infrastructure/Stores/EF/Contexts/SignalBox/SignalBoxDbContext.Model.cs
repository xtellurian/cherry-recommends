using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Integrations.Custom;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Recommenders;
using SignalBox.Infrastructure.EntityFramework;

namespace SignalBox.Infrastructure
{
    public partial class SignalBoxDbContext : DbContextBase
    {
        // core stuff
        public DbSet<Core.Environment> Environments { get; set; }
        public DbSet<RecommendableItem> RecommendableItems { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerEvent> CustomerEvents { get; set; }
        public DbSet<TrackedUserAction> TrackedUserActions { get; set; }
        public DbSet<RewardSelector> RewardSelectors { get; set; }
        public DbSet<Touchpoint> Touchpoints { get; set; }
        public DbSet<TrackedUserTouchpoint> TrackedUserTouchpoints { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        // metrics
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricGenerator> MetricGenerators { get; set; }
        public DbSet<HistoricCustomerMetric> HistoricCustomerMetrics { get; set; }
        public DbSet<LatestMetricVersion> LatestFeatureVersions { get; set; } // SQL view

        // recommenders
        public DbSet<RecommenderEntityBase> Recommenders { get; set; }
        public DbSet<ParameterSetRecommender> ParameterSetRecommenders { get; set; }
        public DbSet<ItemsRecommender> ItemsRecommenders { get; set; }

        // ----- destinations -----
        // recommendation destinations
        public DbSet<RecommendationDestinationBase> RecommendationDestinations { get; set; }
        public DbSet<WebhookDestination> WebhookRecommendationDestinations { get; set; }
        public DbSet<SegmentSourceFunctionDestination> SegmentSourceFunctionDestinations { get; set; }

        // feature destinations
        public DbSet<MetricDestinationBase> FeatureDestinations { get; set; }
        public DbSet<WebhookMetricDestination> WebhookFeatureDestinations { get; set; }
        public DbSet<HubspotContactPropertyMetricDestination> HubspotContactPropertyFeatureDestinations { get; set; }

        // recommendations
        public DbSet<RecommendationCorrelator> RecommendationCorrelators { get; set; }
        public DbSet<ParameterSetRecommendation> ParameterSetRecommendations { get; set; }
        public DbSet<ItemsRecommendation> ItemsRecommendations { get; set; }

        // recommender performance reports
        // hierarchy
        public DbSet<PerformanceReportBase> RecommenderPerformanceReports { get; set; }
        public DbSet<ItemsRecommenderPerformanceReport> ItemsRecommenderPerformanceReports { get; set; }

        // integrated systems
        public DbSet<IntegratedSystem> IntegratedSystems { get; set; }
        public DbSet<CustomIntegratedSystem> CustomIntegratedSystems { get; set; }

        // system stuff
        public DbSet<HashedApiKey> ApiKeys { get; set; }
        public DbSet<ModelRegistration> ModelRegistrations { get; set; }
        public DbSet<WebhookReceiver> WebhookReceivers { get; set; }
        public DbSet<TrackedUserSystemMap> TrackUserSystemMaps { get; set; }

    }
}

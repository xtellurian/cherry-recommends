using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Integrations.Custom;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Campaigns;
using SignalBox.Infrastructure.EntityFramework;
using SignalBox.Core.Metrics;
using SignalBox.Core.Segments;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Integrations.Website;

namespace SignalBox.Infrastructure
{
    public partial class SignalBoxDbContext : DbContextBase
    {
        // core stuff
        public DbSet<Core.Environment> Environments { get; set; }
        public DbSet<RecommendableItem> RecommendableItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerEvent> CustomerEvents { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Business> Businesses { get; set; }


        // segments
        public DbSet<Segment> Segments { get; set; }
        public DbSet<CustomerSegment> CustomerSegments { get; set; }
        public DbSet<Audience> Audiences { get; set; }
        public DbSet<EnrolmentRule> EnrolmentRules { get; set; }
        public DbSet<MetricEnrolmentRule> MetricEnrolmentRules { get; set; }

        // metrics
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricGenerator> MetricGenerators { get; set; }
        public DbSet<MetricValueBase> MetricValues { get; set; }
        public DbSet<HistoricCustomerMetric> HistoricCustomerMetrics { get; set; }
        public DbSet<GlobalMetricValue> GlobalMetrics { get; set; }
        public DbSet<BusinessMetricValue> BusinessMetrics { get; set; }
        public DbSet<LatestMetric> LatestMetrics { get; set; } // SQL view
        public DbSet<MetricDailyBinValueNumeric> MetricDailyBinNumericValues { get; set; } // DbSet for executing stored procedure
        public DbSet<MetricDailyBinValueString> MetricDailyBinStringValues { get; set; } // DbSet for executing stored procedure

        // campaigns
        public DbSet<CampaignEntityBase> Campaigns { get; set; }
        public DbSet<ParameterSetCampaign> ParameterSetCampaigns { get; set; }
        public DbSet<PromotionsCampaign> PromotionsCampaigns { get; set; }

        // argument rules
        public DbSet<ArgumentRule> ArgumentRules { get; set; }
        public DbSet<ChoosePromotionArgumentRule> ChoosePromotionArgumentRules { get; set; }
        public DbSet<ChooseSegmentArgumentRule> ChooseSegmentArgumentRules { get; set; }

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

        // offers
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferMeanGrossRevenue> OfferMeanGrossRevenues { get; set; } // DbSet for executing stored procedure
        public DbSet<OfferConversionRateData> OfferConversionRates { get; set; } // DbSet for executing stored procedure
        public DbSet<OfferSensitivityCurveData> OfferSensitivityCurveData { get; set; } // DbSet for executing stored procedure

        // discount codes
        public DbSet<DiscountCode> DiscountCodes { get; set; }

        // promotion channels
        public DbSet<ChannelBase> Channels { get; set; }
        public DbSet<WebhookChannel> WebhookChannels { get; set; }
        public DbSet<WebChannel> WebChannels { get; set; }
        public DbSet<EmailChannel> EmailChannels { get; set; }

        // channel delivery
        public DbSet<DeferredDelivery> DeferredDeliveries { get; set; }

        // optimisers
        public DbSet<PromotionOptimiser> PromotionOptimisers { get; set; }

        // recommender performance reports
        // hierarchy
        public DbSet<PerformanceReportBase> RecommenderPerformanceReports { get; set; }
        public DbSet<ItemsRecommenderPerformanceReport> ItemsRecommenderPerformanceReports { get; set; }

        // integrated systems
        public DbSet<IntegratedSystem> IntegratedSystems { get; set; }
        public DbSet<CustomIntegratedSystem> CustomIntegratedSystems { get; set; }
        public DbSet<WebsiteIntegratedSystem> WebsiteIntegratedSystems { get; set; }
        public DbSet<IntegratedSystemCredential> IntegratedSystemCredentials { get; set; }

        // system stuff
        public DbSet<HashedApiKey> ApiKeys { get; set; }
        public DbSet<ModelRegistration> ModelRegistrations { get; set; }
        public DbSet<WebhookReceiver> WebhookReceivers { get; set; }
        public DbSet<TrackedUserSystemMap> TrackUserSystemMaps { get; set; }

        // views
        public DbSet<CustomerMetricDailyNumericAggregate> CustomerMetricDailyNumericAggregates { get; set; }
        public DbSet<CustomerMetricWeeklyNumericAggregate> CustomerMetricWeeklyNumericAggregates { get; set; }
        public DbSet<CustomerMetricDailyStringAggregate> CustomerMetricDailyStringAggregates { get; set; }
        public DbSet<CustomerMetricWeeklyStringAggregate> CustomerMetricWeeklyStringAggregates { get; set; }
    }
}

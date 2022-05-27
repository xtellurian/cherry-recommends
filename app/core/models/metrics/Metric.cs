using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public class Metric : CommonEntity
    {
        // Metrics Default Data
        private const string revenueCommonId = "revenue";
        private const string totalEventsCommonId = "total_events";
        private const long revenueId = 100;
        private const long totalEventsId = 101;
        public static string RevenueCommonId => revenueCommonId;
        public static string TotalEventsCommonId => totalEventsCommonId;
        public static Metric Revenue => new Metric(revenueCommonId, "Revenue", MetricValueType.Numeric, MetricScopes.Customer)
        {
            Id = revenueId // need to include the primary key for EF Core reasons
        };
        public static Metric TotalEvents => new Metric(totalEventsCommonId, "Total Events", MetricValueType.Numeric, MetricScopes.Customer)
        {
            Id = totalEventsId // need to include the primary key for EF Core reasons
        };

        [JsonConstructor]
        public Metric() // this ensures the object can be deserialized between dotnetFunctions and the server
        { }
        public Metric(string commonId, string name, MetricValueType? valueType, MetricScopes scope) : base(commonId, name)
        {
            ValueType = valueType;
            Scope = scope;
        }

        // properties
        public MetricValueType? ValueType { get; set; }
        public MetricScopes Scope { get; set; }

        [JsonIgnore]
        public ICollection<CampaignEntityBase> Recommenders { get; set; }
        [JsonIgnore]
        public ICollection<HistoricCustomerMetric> HistoricTrackedUserFeatures { get; set; }
        [JsonIgnore]
        public ICollection<MetricDestinationBase> Destinations { get; set; }
    }
}
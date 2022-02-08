using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Recommenders;

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
        public static Metric Revenue => new Metric(revenueCommonId, "Revenue", MetricValueType.Numeric)
        {
            Id = revenueId // need to include the primary key for EF Core reasons
        };
        public static Metric TotalEvents => new Metric(totalEventsCommonId, "Total Events", MetricValueType.Numeric)
        {
            Id = totalEventsId // need to include the primary key for EF Core reasons
        };

        protected Metric()
        { }
        public Metric(string commonId, string name, MetricValueType valueType) : base(commonId, name)
        {
            ValueType = valueType;
        }

        // properties
        public MetricValueType? ValueType { get; set; }

        [JsonIgnore]
        public ICollection<RecommenderEntityBase> Recommenders { get; set; }
        [JsonIgnore]
        public ICollection<HistoricCustomerMetric> HistoricTrackedUserFeatures { get; set; }
        [JsonIgnore]
        public ICollection<MetricDestinationBase> Destinations { get; set; }
    }
}
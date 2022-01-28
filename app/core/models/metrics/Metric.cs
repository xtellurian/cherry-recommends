using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class Metric : CommonEntity
    {
        public Metric()
        { }
        public Metric(string commonId, string name) : base(commonId, name)
        { }

        [JsonIgnore]
        public ICollection<RecommenderEntityBase> Recommenders { get; set; }
        [JsonIgnore]
        public ICollection<HistoricCustomerMetric> HistoricTrackedUserFeatures { get; set; }
        [JsonIgnore]
        public ICollection<MetricDestinationBase> Destinations { get; set; }
    }
}
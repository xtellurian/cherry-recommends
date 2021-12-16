using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class HistoricTrackedUserFeature : TrackedUserFeatureBase
    {
        protected HistoricTrackedUserFeature()
        { }

        public HistoricTrackedUserFeature(Customer customer, Feature feature, string value, int version)
        : base(customer, feature, value)
        {
            Version = version;
        }

        public HistoricTrackedUserFeature(Customer customer, Feature feature, double value, int version)
        : base(customer, feature, value)
        {
            Version = version;
        }

#nullable enable
        public int Version { get; set; }
    }
}
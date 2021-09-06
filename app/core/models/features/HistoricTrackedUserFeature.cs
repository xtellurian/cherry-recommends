using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class HistoricTrackedUserFeature : TrackedUserFeatureBase
    {
        protected HistoricTrackedUserFeature()
        { }
        private HistoricTrackedUserFeature(TrackedUser trackedUser, Feature feature, int version) : base(trackedUser, feature)
        {
            Version = version;
        }
        public HistoricTrackedUserFeature(TrackedUser trackedUser, Feature feature, string value, int version)
        : this(trackedUser, feature, version)
        {
            StringValue = value;
        }

        public HistoricTrackedUserFeature(TrackedUser trackedUser, Feature feature, double value, int version)
        : this(trackedUser, feature, version)
        {
            NumericValue = value;
        }

#nullable enable
        public int Version { get; set; }
    }
}
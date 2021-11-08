using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class HistoricTrackedUserFeature : TrackedUserFeatureBase
    {
        protected HistoricTrackedUserFeature()
        { }

        public HistoricTrackedUserFeature(TrackedUser trackedUser, Feature feature, string value, int version)
        : base(trackedUser, feature, value)
        {
            Version = version;
        }

        public HistoricTrackedUserFeature(TrackedUser trackedUser, Feature feature, double value, int version)
        : base(trackedUser, feature, value)
        {
            Version = version;
        }

#nullable enable
        public int Version { get; set; }
    }
}
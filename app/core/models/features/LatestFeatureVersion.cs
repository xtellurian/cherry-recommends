namespace SignalBox.Core
{
    // this class is mapped to a SQL View
    // The view is created in migration develop_historic_features_view

    public class LatestFeatureVersion
    {
        public long HistoricTrackedUserFeatureId { get; set; }
        public long TrackedUserId { get; set; }
        public long FeatureId { get; set; }
        public int MaxVersion { get; set; }
    }
}
namespace SignalBox.Core
{
    // this class is mapped to a SQL View
    // The view is created in migration develop_historic_features_view

    public class LatestMetricVersion
    {
        public long HistoricCustomerMetricId { get; set; }
        public long CustomerId { get; set; }
        public long MetricId { get; set; }
        public int MaxVersion { get; set; }
    }
}
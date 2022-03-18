namespace SignalBox.Core
{
    // this class is mapped to a SQL View
    // The view is created in migration develop_historic_features_view
#nullable enable
    public class LatestMetric
    {
        public long HistoricCustomerMetricId { get; set; }
        public long? CustomerId { get; set; }
        public long? BusinessId { get; set; }
        public long MetricId { get; set; }
        public int MaxVersion { get; set; }
        public double? NumericValue { get; set; }
        public string? StringValue { get; set; }
    }
}
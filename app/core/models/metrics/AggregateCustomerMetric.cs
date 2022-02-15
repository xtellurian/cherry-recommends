namespace SignalBox.Core.Metrics
{
    public class AggregateCustomerMetric
    {
        public long MetricId { get; set; }
        public Metric Metric { get; set; }
        public AggregationTypes AggregationType { get; set; }
        public string CategoricalValue { get; set; } // value to sum (i.e. count) if the other metric is a categorical metric
    }
}
namespace SignalBox.Core.Metrics
{
    public class GlobalMetricValue : MetricValueBase
    {
        protected GlobalMetricValue() { }
        public GlobalMetricValue(Metric metric, int version, double value) : base(metric, version, value)
        {
            if (metric.Scope != MetricScopes.Global)
            {
                throw new BadRequestException("Global Metric Values can only be created for metrics with global scope.");
            }
        }

        public GlobalMetricValue(Metric metric, int version, string value) : base(metric, version, value)
        {
            if (metric.Scope != MetricScopes.Global)
            {
                throw new BadRequestException("Global Metric Values can only be created for metrics with global scope.");
            }
        }
    }
}

namespace SignalBox.Core
{
    public class RunMetricGeneratorQueueMessage : RunAllMetricGeneratorsQueueMessage
    {
        public RunMetricGeneratorQueueMessage()
        { }
        public RunMetricGeneratorQueueMessage(string tenantName, long metricGeneratorId) : base(tenantName)
        {
            this.MetricGeneratorId = metricGeneratorId;
        }

        public long FeatureGeneratorId => MetricGeneratorId;
        public long MetricGeneratorId { get; set; }

    }
}

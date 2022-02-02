namespace SignalBox.Core
{
    public class RunMetricGeneratorQueueMessage : RunAllMetricGeneratorsQueueMessage
    {
        public RunMetricGeneratorQueueMessage()
        { }
        public RunMetricGeneratorQueueMessage(string tenantName, long metricGeneratorId, long? environmentId) : base(tenantName)
        {
            MetricGeneratorId = metricGeneratorId;
            EnvironmentId = environmentId;
        }

        public long FeatureGeneratorId => MetricGeneratorId;
        public long MetricGeneratorId { get; set; }
        public long? EnvironmentId { get; set; }

    }
}

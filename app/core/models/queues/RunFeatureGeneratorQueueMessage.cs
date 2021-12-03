namespace SignalBox.Core
{
    public class RunFeatureGeneratorQueueMessage : RunAllFeatureGeneratorsQueueMessage
    {
        public RunFeatureGeneratorQueueMessage()
        { }
        public RunFeatureGeneratorQueueMessage(string tenantName, long featureGeneratorId) : base(tenantName)
        {
            this.FeatureGeneratorId = featureGeneratorId;
        }

        public long FeatureGeneratorId { get; set; }

    }
}

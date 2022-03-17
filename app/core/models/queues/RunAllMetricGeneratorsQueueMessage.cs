namespace SignalBox.Core
{
    public class RunAllMetricGeneratorsQueueMessage : TenantJobMessageBase, IQueueMessage
    {
        public RunAllMetricGeneratorsQueueMessage()
        { }
        public RunAllMetricGeneratorsQueueMessage(string tenantName) : base(tenantName)
        { }
    }
}
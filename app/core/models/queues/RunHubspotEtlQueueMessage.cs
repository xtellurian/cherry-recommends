namespace SignalBox.Core
{
    public class RunHubspotEtlQueueMessage : TenantJobMessageBase
    {
        public RunHubspotEtlQueueMessage()
        { }
        public RunHubspotEtlQueueMessage(string tenantName, long integratedSystemId, long? environmentId) : base(tenantName)
        {
            IntegratedSystemId = integratedSystemId;
            EnvironmentId = environmentId;
        }

        public long IntegratedSystemId { get; set; }
        public long? EnvironmentId { get; set; }
    }
}

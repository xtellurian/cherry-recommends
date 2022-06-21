namespace SignalBox.Core
{
    public class RunAllHubspotEtlQueueMessage : TenantJobMessageBase
    {
        public RunAllHubspotEtlQueueMessage(string tenantName) : base(tenantName)
        { }
    }
}
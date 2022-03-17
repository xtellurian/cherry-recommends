namespace SignalBox.Core
{
    /// <summary>
    /// Base class for running a job for a single tenant
    /// </summary>
    public abstract class TenantJobMessageBase : IQueueMessage
    {
        public TenantJobMessageBase()
        { }
        public TenantJobMessageBase(string tenantName)
        {
            TenantName = tenantName;
        }

        public string TenantName { get; set; }
    }
}
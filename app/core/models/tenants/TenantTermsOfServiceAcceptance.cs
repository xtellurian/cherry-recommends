namespace SignalBox.Core
{
#nullable enable
    public class TenantTermsOfServiceAcceptance : Entity
    {
        protected TenantTermsOfServiceAcceptance()
        {
            this.Version = null!;
        }

        public TenantTermsOfServiceAcceptance(Tenant tenant, string version, string acceptedByUserId)
        {
            this.Tenant = tenant;
            this.TenantId = tenant.Id;
            this.Version = version;
            this.AcceptedByUserId = acceptedByUserId;
        }

        public long? TenantId { get; set; }
        public Tenant? Tenant { get; set; }
        public string Version { get; set; }
        public string? AcceptedByUserId { get; set; }
    }
}
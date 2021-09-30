using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TenantMembership : Entity
    {
        protected TenantMembership() { }

        public TenantMembership(Tenant tenant, string userId)
        {
            this.TenantId = tenant.Id;
            this.Tenant = tenant;
            this.UserId = userId;
        }
        public long TenantId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Tenant Tenant { get; set; }
        public string UserId { get; set; }
    }
}
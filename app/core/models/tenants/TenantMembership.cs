using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TenantMembership : Entity
    {
        protected TenantMembership() { }

#nullable enable
        public TenantMembership(Tenant tenant, string userId, string? email)
        {
            TenantId = tenant.Id;
            Tenant = tenant;
            UserId = userId;
            Email = email;
        }
        public long TenantId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Tenant Tenant { get; set; }
        public string UserId { get; set; }
        public string? Email { get; set; }
    }
}
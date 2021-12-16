using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUserSystemMap : Entity
    {
        public TrackedUserSystemMap()
        {
        }

        public TrackedUserSystemMap(string userId, IntegratedSystem integratedSystem, Customer customer)
        {
            UserId = userId;
            IntegratedSystem = integratedSystem;
            Customer = customer;
        }

        public string UserId { get; set; }
        public long IntegratedSystemId { get; protected set; }
        [JsonIgnore]
        public IntegratedSystem IntegratedSystem { get; set; }
        [JsonIgnore]
        public long TrackedUserId { get; protected set; }
        [JsonIgnore]
        public Customer Customer { get; set; }

        // this is a property for API backwards compat
        [JsonIgnore]
        public Customer TrackedUser => Customer;
    }
}
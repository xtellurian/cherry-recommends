using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class BusinessMembership
    {
        public long BusinessId { get; set; }
        public Business Business { get; set; }
        public long CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }
    }
}
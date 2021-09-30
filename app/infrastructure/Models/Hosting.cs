using System.Text.Json.Serialization;

namespace SignalBox.Infrastructure.Models
{
    public class Hosting
    {
        public bool Multitenant { get; set; }
        public string CanonicalRootDomain { get; set; }
        [JsonIgnore]
        public string SingleTenantDatabaseName { get; set; }
    }
}
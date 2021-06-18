using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{
    public partial class HubspotAccountDetails
    {
        [JsonPropertyName("portalId")]
        public long PortalId { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("utcOffset")]
        public string UtcOffset { get; set; }

        [JsonPropertyName("utcOffsetMilliseconds")]
        public long UtcOffsetMilliseconds { get; set; }
    }
}
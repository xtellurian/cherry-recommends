using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{
#nullable enable
    public partial class HubspotWebhookPayload
    {
        [JsonPropertyName("objectId")]
        public long? ObjectId { get; set; }

        [JsonPropertyName("propertyName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PropertyName { get; set; }

        [JsonPropertyName("propertyValue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PropertyValue { get; set; }

        [JsonPropertyName("changeSource")]
        public string? ChangeSource { get; set; }

        [JsonPropertyName("eventId")]
        public long? EventId { get; set; }

        [JsonPropertyName("subscriptionId")]
        public long? SubscriptionId { get; set; }

        [JsonPropertyName("portalId")]
        public long? PortalId { get; set; }

        [JsonPropertyName("appId")]
        public long? AppId { get; set; }

        [JsonPropertyName("occurredAt")]
        public long? OccurredAt { get; set; }

        [JsonPropertyName("subscriptionType")]
        public string? SubscriptionType { get; set; }

        [JsonPropertyName("attemptNumber")]
        public long? AttemptNumber { get; set; }
    }
}


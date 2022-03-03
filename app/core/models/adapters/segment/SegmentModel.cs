using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Segment
{
#nullable enable
    public class SegmentModel
    {
        [JsonPropertyName("version")]
        public double Version { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; } = null!;

        [JsonPropertyName("event")]
        public string? Event { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, object>? Properties { get; set; }
        [JsonPropertyName("traits")]
        public Dictionary<string, object>? Traits { get; set; }

        [JsonPropertyName("groupId")]
        public string? GroupId { get; set; }
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
        [JsonPropertyName("anonymousId")]
        public string? AnonymousId { get; set; }
    }
}
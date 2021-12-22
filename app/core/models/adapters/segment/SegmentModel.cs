using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Segment
{
    public class SegmentModel
    {
        [JsonPropertyName("version")]
        public long Version { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, object> Properties { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("anonymousId")]
        public string AnonymousId { get; set; }
    }
}
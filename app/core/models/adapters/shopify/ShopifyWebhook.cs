using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class ShopifyWebhook : ShopifyObjectBase
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [JsonPropertyName("fields")]
        public IEnumerable<string> Fields { get; set; }
        [JsonPropertyName("format")]
        public string Format { get; set; }
        [JsonPropertyName("metafield_namespaces")]
        public IEnumerable<string> MetafieldNamespaces { get; set; }
        [JsonPropertyName("topic")]
        public string Topic { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
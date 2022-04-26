using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class ShopifyWebhook : ShopifyObjectBase
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }
        [JsonPropertyName("fields")]
        public IEnumerable<string> Fields { get; set; }
        [JsonPropertyName("format")]
        public string Format { get; set; }
        [JsonPropertyName("metafieldNamespaces")]
        public IEnumerable<string> MetafieldNamespaces { get; set; }
        [JsonPropertyName("topic")]
        public string Topic { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }
        [JsonPropertyName("endpoint")]
        public WebhookSubscriptionEndpoint Endpoint { get; set; }
    }

    public class WebhookSubscriptionEndpoint
    {
        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; set; }
    }
}
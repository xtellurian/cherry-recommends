using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class ShopifyWebhookCreateResponse : IMutationResponse<WebhookSubscriptionCreate>
    {
        [JsonPropertyName("webhookSubscriptionCreate")]
        public WebhookSubscriptionCreate Mutation { get; set; }
    }

    public class WebhookSubscriptionCreate : IMutationResponseData
    {
        [JsonPropertyName("userErrors")]
        public UserError[] UserErrors { get; set; }
        [JsonPropertyName("webhookSubscription")]
        public ShopifyWebhook WebhookSubscription { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class ShopifySubscriptionEvent
    {
        /// <summary>
        /// The name of the recurring application charge.
        /// </summary>
        [JsonPropertyName("app_subscription")]
        public ShopifyRecurringCharge AppSubscription { get; set; }
    }
}
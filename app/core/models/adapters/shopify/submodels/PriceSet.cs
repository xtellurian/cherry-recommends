using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class PriceSet
    {
        [JsonPropertyName("shop_money")]
        public Price ShopMoney { get; set; }

        [JsonPropertyName("presentment_money")]
        public Price PresentmentMoney { get; set; }
    }
}
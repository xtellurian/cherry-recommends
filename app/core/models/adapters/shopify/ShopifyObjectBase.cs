using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public abstract class ShopifyObjectBase
    {
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        [JsonPropertyName("id")]
        public long? Id { get; set; }
        [JsonPropertyName("admin_graphql_api_id")]
        public string AdminGraphQLAPIId { get; set; }
    }
}

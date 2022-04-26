using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class UserError
    {
        [JsonPropertyName("field")]
        public string[] Field { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
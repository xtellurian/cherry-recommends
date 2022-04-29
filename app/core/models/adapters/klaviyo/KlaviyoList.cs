using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Klaviyo
{
    public class KlaviyoList
    {
        [JsonPropertyName("list_id")]
        public string ListId { get; set; }

        [JsonPropertyName("list_name")]
        public string ListName { get; set; }
    }
}
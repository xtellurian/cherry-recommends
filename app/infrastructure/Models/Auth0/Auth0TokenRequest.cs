using System.Text.Json.Serialization;

namespace SignalBox.Infrastructure
{
    public partial class Auth0TokenRequest
    {
        [JsonPropertyName("audience")]
        public string Audience { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}

using System.Text.Json.Serialization;
using SignalBox.Core.OAuth;

namespace SignalBox.Infrastructure
{
    public partial class Auth0TokenResponse : TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}

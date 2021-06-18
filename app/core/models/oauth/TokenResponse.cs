using System.Text.Json.Serialization;

namespace SignalBox.Core.OAuth
{
    public class TokenResponse
    {
        public TokenResponse()
        { }
        public TokenResponse(string accessToken, string refreshToken, string scope, long expiresIn)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Scope = scope;
            ExpiresIn = expiresIn;
        }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

    }
}
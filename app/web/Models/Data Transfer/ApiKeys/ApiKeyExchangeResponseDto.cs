using System.Text.Json.Serialization;

namespace SignalBox.Web.Dto
{
    public class ApiKeyExchangeResponseDto
    {
        public ApiKeyExchangeResponseDto(string accessToken)
        {
            AccessToken = accessToken;
        }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
using Newtonsoft.Json;

namespace SignalBox.Client
{
    public class JavascriptConfiguration
    {
        public JavascriptConfiguration()
        {
        }

        public JavascriptConfiguration(string baseUrl, AuthenticationResponse auth)
        {
            BaseUrl = baseUrl;
            Auth = auth;
        }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("auth")]
        public AuthenticationResponse Auth { get; set; }
    }
}
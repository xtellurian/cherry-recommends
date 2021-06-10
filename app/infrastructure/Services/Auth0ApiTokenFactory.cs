using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Services
{
    public class Auth0ApiTokenFactory : IApiTokenFactory
    {
        private readonly IOptions<Auth0M2MClient> options;
        private readonly HttpClient httpClient;

        public Auth0ApiTokenFactory(IOptions<Auth0M2MClient> options, HttpClient httpClient)
        {
            this.options = options;
            this.httpClient = httpClient;
        }

        public async Task<string> GetToken()
        {
            var request = new Auth0TokenRequest()
            {
                Audience = options.Value.Audience,
                GrantType = "client_credentials",
                ClientId = options.Value.ClientId,
                ClientSecret = options.Value.ClientSecret,
            };

            var response = await httpClient.PostAsJsonAsync(options.Value.Endpoint, request);
            
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<Auth0TokenResponse>(responseContent);
            return tokenResponse.AccessToken;
        }
    }
}
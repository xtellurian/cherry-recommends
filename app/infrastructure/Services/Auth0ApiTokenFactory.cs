using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.OAuth;

namespace SignalBox.Infrastructure.Services
{
    public class Auth0ApiTokenFactory : IApiTokenFactory
    {
        private readonly IOptions<Auth0M2MClient> m2mOptions;
        private readonly IOptions<Auth0ManagementCredentials> managementOptions;
        private readonly HttpClient httpClient;

        public Auth0ApiTokenFactory(IOptions<Auth0M2MClient> m2mOptions,
                                    IOptions<Auth0ManagementCredentials> managementOptions,
                                    HttpClient httpClient)
        {
            this.m2mOptions = m2mOptions;
            this.managementOptions = managementOptions;
            this.httpClient = httpClient;
        }

        public async Task<TokenResponse> GetM2MToken(string scope = null)
        {
            var request = new Auth0TokenRequest()
            {
                Audience = m2mOptions.Value.Audience,
                GrantType = "client_credentials",
                ClientId = m2mOptions.Value.ClientId,
                ClientSecret = m2mOptions.Value.ClientSecret,
                Scope = scope
            };
            return await GetToken(m2mOptions.Value.Endpoint, request);
        }

        public async Task<TokenResponse> GetManagementToken(string scope = null)
        {
            var request = new Auth0TokenRequest()
            {
                Audience = $"https://{managementOptions.Value.Domain}/api/v2/",
                GrantType = "client_credentials",
                ClientId = managementOptions.Value.ClientId,
                ClientSecret = managementOptions.Value.ClientSecret,
                Scope = scope
            };

            return await GetToken(managementOptions.Value.Endpoint, request);
        }

        private async Task<TokenResponse> GetToken(string endpoint, Auth0TokenRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(endpoint, request);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var tokenResponse = JsonSerializer.Deserialize<Auth0TokenResponse>(responseContent);
            return tokenResponse;
        }

    }
}
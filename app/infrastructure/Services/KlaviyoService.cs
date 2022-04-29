using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Klaviyo;
using SignalBox.Core.Integrations;
using SignalBox.Core.Serialization;

namespace SignalBox.Infrastructure.Services
{
    public class KlaviyoService : IKlaviyoService
    {
        private const string BaseUrl = "https://a.klaviyo.com/api";
        private readonly HttpClient httpClient;
        private readonly ILogger<KlaviyoService> logger;

        public KlaviyoService(HttpClient httpClient, ILogger<KlaviyoService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<IEnumerable<KlaviyoList>> GetLists(KlaviyoApiKeys apiKeys)
        {
            var listEndpoint = $"{BaseUrl}/v2/lists?api_key={apiKeys.PrivateKey}";
            var response = await httpClient.GetAsync(listEndpoint);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var klaviyoLists = Serializer.Deserialize<List<KlaviyoList>>(content);

            return klaviyoLists;
        }
    }
}


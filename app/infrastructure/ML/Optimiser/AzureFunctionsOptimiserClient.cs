using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.ML
{
    public class AzureFunctionsOptimiserClient : ICategoricalOptimiserClient
    {
        private readonly ITenantProvider tenantProvider;

        public DotnetAzureFunctionsConnectionOptions connectionOptions { get; }

        private JsonSerializerOptions serializerOptions => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public AzureFunctionsOptimiserClient(
            IOptions<DotnetAzureFunctionsConnectionOptions> options,
            HttpClient httpClient,
            ITenantProvider tenantProvider)
        {
            httpClient.BaseAddress = new System.Uri(options.Value.Url);
            httpClient.DefaultRequestHeaders.Add("x-functions-key", options.Value.Key);
            this.httpClient = httpClient;
            this.tenantProvider = tenantProvider;
            this.connectionOptions = options.Value;
        }

        public HttpClient httpClient { get; }

        public async Task<CategoricalOptimiser> Create(IRecommender recommender)
        {
            var tenant = tenantProvider.Current();
            var tenantName = tenant?.Name ?? "single-tenant";
            var itemsRecommender = (ItemsRecommender)recommender;
            var optimiser = new CategoricalOptimiser
            {
                Id = System.Guid.NewGuid().ToString(),
                BaselineItem = itemsRecommender.BaselineItem,
                Items = itemsRecommender.Items,
                Name = itemsRecommender.ModelRegistration.Name,
                NItemsToRecommend = itemsRecommender.NumberOfItemsToRecommend ?? 1
            };

            var serialized = JsonSerializer.Serialize(optimiser, serializerOptions);
            var inputContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var r = await httpClient.PostAsync($"api/v1/{tenantName}/categorical", inputContent);
            var content = await r.Content.ReadAsStringAsync();
            if (!r.IsSuccessStatusCode)
            {
                throw new OptimiserException("Error creating optimiser", content);
            }

            optimiser = JsonSerializer.Deserialize<CategoricalOptimiser>(content, serializerOptions);
            recommender.ModelRegistration.ScoringUrl = httpClient.BaseAddress.ToString()
                + $"api/v1/{tenantName}/categorical/{optimiser.Id}/invoke";
            recommender.ModelRegistration.Key = connectionOptions.Key;
            return optimiser;
        }
    }
}
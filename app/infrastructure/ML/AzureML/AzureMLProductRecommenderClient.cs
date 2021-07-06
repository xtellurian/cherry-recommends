using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class AzureMLPProductRecommenderClient : MLModelClient, IModelClient<ProductRecommenderModelInputV1, ProductRecommenderModelOutputV1>
    {
        private readonly HttpClient httpClient;

        public AzureMLPProductRecommenderClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            base.SetApplicationJsonHeader(this.httpClient);
        }

        public async Task<ProductRecommenderModelOutputV1> Invoke(ModelRegistration model, string version, ProductRecommenderModelInputV1 input)
        {
            try
            {
                base.SetKeyAsBearerToken(httpClient, model);
                var response = await httpClient.PostAsJsonAsync(model.ScoringUrl,
                    new ModelInputWrapper<ProductRecommenderModelInputV1>(version, input), serializerOptions);
                var body = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<ProductRecommenderModelOutputV1>(body, serializerOptions);
            }
            catch (System.Exception ex)
            {
                throw new ModelInvokationException(model, ex);
            }
        }
    }
}
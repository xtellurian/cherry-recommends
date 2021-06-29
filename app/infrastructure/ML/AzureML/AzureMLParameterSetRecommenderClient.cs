using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class AzureMLParameterSetRecommenderClient : MLModelClient, IModelClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1>
    {
        private readonly HttpClient httpClient;

        public AzureMLParameterSetRecommenderClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            base.SetApplicationJsonHeader(this.httpClient);
        }

        public async Task<ParameterSetRecommenderModelOutputV1> Invoke(ModelRegistration model, string version, ParameterSetRecommenderModelInputV1 input)
        {
            try
            {
                base.SetKeyAsBearerToken(httpClient, model);
                var response = await httpClient.PostAsJsonAsync(model.ScoringUrl, 
                    new ModelInputWrapper<ParameterSetRecommenderModelInputV1>(version, input), serializerOptions);
                var body = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<ParameterSetRecommenderModelOutputV1>(body, serializerOptions);
            }
            catch (System.Exception ex)
            {
                throw new ModelInvokationException(model, ex);
            }
        }
    }
}
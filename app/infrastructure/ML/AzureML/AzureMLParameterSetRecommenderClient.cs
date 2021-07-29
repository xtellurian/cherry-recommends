using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class AzureMLParameterSetRecommenderClient : MLModelClient, IRecommenderModelClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1>
    {
        private readonly HttpClient httpClient;

        public AzureMLParameterSetRecommenderClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.Timeout = new System.TimeSpan(0, 0, 2); // 2 second timeout
            base.SetApplicationJsonHeader(this.httpClient);
        }

        public async Task<ParameterSetRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext recommendingContext, ParameterSetRecommenderModelInputV1 input)
        {
            string body = null;
            try
            {
                base.SetKeyAsBearerToken(httpClient, recommender.ModelRegistration);
                var response = await httpClient.PostAsJsonAsync(recommender.ModelRegistration.ScoringUrl,
                    new ModelInputWrapper<ParameterSetRecommenderModelInputV1>(recommendingContext.Version, input), serializerOptions);
                body = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<ParameterSetRecommenderModelOutputV1>(body, serializerOptions);
            }
            catch (System.OperationCanceledException operationCancelled)
            {
                throw new ModelInvokationException(recommender.ModelRegistration, operationCancelled, "The underlying model request probably timed out.");
            }
            catch (System.Exception ex)
            {
                throw new ModelInvokationException(recommender.ModelRegistration, ex, body);
            }
        }
    }
}
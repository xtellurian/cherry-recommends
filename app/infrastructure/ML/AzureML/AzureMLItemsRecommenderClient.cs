using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class AzureMLItemsRecommenderClient : MLModelClient, IRecommenderModelClient<ModelInputDto, ItemsRecommenderModelOutputV1>
    {
        private readonly HttpClient httpClient;

        public AzureMLItemsRecommenderClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.Timeout = new System.TimeSpan(0, 0, 2); // 2 second timeout
            base.SetApplicationJsonHeader(this.httpClient);
        }

        public async Task<ItemsRecommenderModelOutputV1> Invoke(IRecommender recommender,
                                                                  RecommendingContext recommendingContext,
                                                                  ModelInputDto input)
        {
            string body = null;
            try
            {
                base.SetKeyAsBearerToken(httpClient, recommender.ModelRegistration);
                var response = await httpClient.PostAsJsonAsync(recommender.ModelRegistration.ScoringUrl,
                    new ModelInputWrapper<ModelInputDto>(recommendingContext.Version, input, recommendingContext.Correlator?.Id), serializerOptions);
                body = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<ItemsRecommenderModelOutputV1>(body, serializerOptions);
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

        public Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action)
        {
            context.Logger.LogWarning($"{this.GetType()} cannot be rewarded");
            return Task.CompletedTask;
        }
    }
}
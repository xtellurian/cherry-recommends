using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class AzureMLParameterSetRecommenderClient : MLModelClient, IRecommenderModelClient<ParameterSetRecommenderModelOutputV1>
    {
        private readonly HttpClient httpClient;

        public AzureMLParameterSetRecommenderClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.Timeout = new System.TimeSpan(0, 0, 2); // 2 second timeout
            base.SetApplicationJsonHeader(this.httpClient);
        }

        public async Task<ParameterSetRecommenderModelOutputV1> Invoke(ICampaign recommender, RecommendingContext recommendingContext, IModelInput input)
        {
            string body = null;
            try
            {
                base.SetKeyAsBearerToken(httpClient, recommender.ModelRegistration);
                var response = await httpClient.PostAsJsonAsync(recommender.ModelRegistration.ScoringUrl,
                    new ModelInputWrapper<IModelInput>(input, recommendingContext.Correlator?.Id), serializerOptions);
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

        public Task Reward(ICampaign campaign, RewardingContext context)
        {
            context.Logger.LogWarning("{type} cannot be rewarded", this.GetType());
            return Task.CompletedTask;
        }
    }
}
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class DotnetFunctionsOptimiserRecommenderClient : MLModelClient, IRecommenderModelClient<ItemsRecommenderModelOutputV1>
    {
        private readonly ITenantProvider tenantProvider;

        public DotnetFunctionsOptimiserRecommenderClient(
            HttpClient httpClient,
            ITenantProvider tenantProvider)
        {
            this.httpClient = httpClient;
            this.tenantProvider = tenantProvider;
        }

        public HttpClient httpClient { get; }

        public async Task<ItemsRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext context, IModelInput input)
        {
            var tenant = tenantProvider.Current();
            httpClient.DefaultRequestHeaders.Add("x-functions-key", recommender.ModelRegistration.Key);
            string body = null;
            try
            {
                var serialized = JsonSerializer.Serialize(new ModelInputWrapper<IModelInput>(input, context.Correlator?.Id), serializerOptions);
                var inputContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(recommender.ModelRegistration.ScoringUrl, inputContent);
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
            throw new System.NotImplementedException();
        }
    }
}
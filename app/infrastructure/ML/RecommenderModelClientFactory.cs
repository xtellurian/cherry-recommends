using System;
using System.Net.Http;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure.ML.Azure;

namespace SignalBox.Infrastructure.ML
{
    public class RecommenderModelClientFactory : IRecommenderModelClientFactory
    {
        private readonly HttpClient httpClient;
        private readonly ITelemetry telemetry;
        private readonly ITenantProvider tenantProvider;
        private readonly IRecommendableItemStore itemStore;

        public RecommenderModelClientFactory(HttpClient httpClient,
                                             ITelemetry telemetry,
                                             ITenantProvider tenantProvider,
                                             IRecommendableItemStore itemStore)
        {
            this.httpClient = httpClient;
            this.telemetry = telemetry;
            this.tenantProvider = tenantProvider;
            this.itemStore = itemStore;
        }

        public Task<IRecommenderModelClient<TOutput>> GetClient<TOutput>(ICampaign recommender)
            where TOutput : IModelOutput
        {
            var model = recommender.ModelRegistration;
            if (model == null)
            {
                throw new ArgumentException("You should call GetUnregisteredClient when there is no linked Model Registration");
            }
            if (recommender.ModelRegistration.HostingType == HostingTypes.AzureFunctions && model.ModelType == ModelTypes.ItemsRecommenderV1)
            {
                return Task.FromResult((IRecommenderModelClient<TOutput>)
                    new DotnetFunctionsOptimiserRecommenderClient(httpClient, tenantProvider));
            }
            if (recommender.ModelRegistration.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.ParameterSetRecommenderV1)
            {
                return Task.FromResult((IRecommenderModelClient<TOutput>)
                    new AzureMLParameterSetRecommenderClient(httpClient));
            }
            else if (model.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.ItemsRecommenderV1)
            {
                return Task.FromResult((IRecommenderModelClient<TOutput>)
                    new AzureMLItemsRecommenderClient(httpClient));
            }
            else
            {
                throw new NotImplementedException("That Hosting Type and Model Type is not supported.");
            }
        }

        public Task<IRecommenderModelRewardClient> GetRewardClient(ICampaign recommender)
        {
            return Task.FromResult((IRecommenderModelRewardClient)new DefaultRewardClient());
        }

        public Task<IRecommenderModelClient<TOutput>> GetUnregisteredClient<TOutput>(ICampaign recommender)
            where TOutput : IModelOutput
        {
            if (typeof(TOutput) == typeof(ParameterSetRecommenderModelOutputV1))
            {
                return Task.FromResult((IRecommenderModelClient<TOutput>)new RandomParameterSetRecommender());
            }
            else
            {
                throw new NotImplementedException("This type of client is not creatable");
            }
        }

        public Task<IRecommenderModelClient<TOutput>> GetUnregisteredItemsRecommenderClient<TOutput>(ICampaign recommender) where TOutput : IModelOutput
        {
            return Task.FromResult((IRecommenderModelClient<TOutput>)new RandomItemsRecommender(itemStore));
        }
    }
}
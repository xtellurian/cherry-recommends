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
        private readonly IProductRecommenderStore productRecommenderStore;
        private readonly IProductStore productStore;

        public RecommenderModelClientFactory(HttpClient httpClient,
                                             ITelemetry telemetry,
                                             IProductRecommenderStore productRecommenderStore,
                                             IProductStore productStore)
        {
            this.httpClient = httpClient;
            this.telemetry = telemetry;
            this.productRecommenderStore = productRecommenderStore;
            this.productStore = productStore;
        }

        public Task<IRecommenderModelClient<TInput, TOutput>> GetClient<TInput, TOutput>(IRecommender recommender)
            where TInput : IModelInput
            where TOutput : IModelOutput
        {
            var model = recommender.ModelRegistration;
            if (model == null)
            {
                throw new ArgumentException("You should call GetUnregisteredClient when there is no linked Model Registration");
            }
            if (recommender.ModelRegistration.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.ParameterSetRecommenderV1)
            {
                return Task.FromResult((IRecommenderModelClient<TInput, TOutput>)
                    new AzureMLParameterSetRecommenderClient(httpClient));
            }
            else if (model.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.ProductRecommenderV1)
            {
                return Task.FromResult((IRecommenderModelClient<TInput, TOutput>)
                    new AzureMLPProductRecommenderClient(httpClient));
            }
            else if (model.HostingType == HostingTypes.AzurePersonalizer && model.ModelType == ModelTypes.ProductRecommenderV1)
            {
                return Task.FromResult((IRecommenderModelClient<TInput, TOutput>)
                    new AzurePersonalizerRecommenderClient(httpClient, productRecommenderStore, productStore, telemetry));
            }
            else
            {
                throw new NotImplementedException("That Hosting Type and Model Type is not supported.");
            }
        }

        public Task<IRecommenderModelRewardClient> GetRewardClient(IRecommender recommender)
        {
            if (recommender.ModelRegistration != null && recommender.ModelRegistration.HostingType == HostingTypes.AzurePersonalizer)
            {
                return Task.FromResult((IRecommenderModelRewardClient)new AzurePersonalizerRecommenderClient(httpClient, productRecommenderStore, productStore, telemetry));
            }
            else
            {
                return Task.FromResult((IRecommenderModelRewardClient)new DefaultRewardClient());
            }
        }

        public Task<IRecommenderModelClient<TInput, TOutput>> GetUnregisteredClient<TInput, TOutput>(IRecommender recommender)
            where TInput : IModelInput
            where TOutput : IModelOutput
        {
            if (typeof(TOutput) == typeof(ProductRecommenderModelOutputV1))
            {
                return Task.FromResult((IRecommenderModelClient<TInput, TOutput>)new RandomProductRecommender(productStore));
            }
            else if (typeof(TOutput) == typeof(ParameterSetRecommenderModelOutputV1))
            {
                return Task.FromResult((IRecommenderModelClient<TInput, TOutput>)new RandomParameterSetRecommender());
            }
            else
            {
                throw new NotImplementedException("This type of client is not creatable");
            }
        }
    }
}
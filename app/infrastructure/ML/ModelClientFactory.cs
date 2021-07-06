using System;
using System.Net.Http;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure.ML.Azure;

namespace SignalBox.Infrastructure.ML
{
    public class ModelClientFactory : IModelClientFactory
    {
        private readonly HttpClient httpClient;
        private readonly IProductStore productStore;

        public ModelClientFactory(HttpClient httpClient, IProductStore productStore)
        {
            this.httpClient = httpClient;
            this.productStore = productStore;
        }

        public Task<IModelClient<TInput, TOutput>> GetClient<TInput, TOutput>(ModelRegistration model)
            where TInput : IModelInput
            where TOutput : IModelOutput
        {
            if (model.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.SingleClassClassifier)
            {
                return Task.FromResult((IModelClient<TInput, TOutput>)new AzureMLClassifierClient(httpClient));
            }
            else if (model.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.ParameterSetRecommenderV1)
            {
                return Task.FromResult((IModelClient<TInput, TOutput>)new AzureMLParameterSetRecommenderClient(httpClient));
            }
            else if (model.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.ProductRecommenderV1)
            {
                return Task.FromResult((IModelClient<TInput, TOutput>)new AzureMLPProductRecommenderClient(httpClient));
            }
            else
            {
                throw new NotImplementedException("That Hosting Type and Model Type is not supported.");
            }
        }

        public Task<IModelClient<TInput, TOutput>> GetUnregisteredClient<TInput, TOutput>()
            where TInput : IModelInput
            where TOutput : IModelOutput
        {
            if (typeof(TInput) == typeof(ProductRecommenderModelInputV1) && typeof(TOutput) == typeof(ProductRecommenderModelOutputV1))
            {
                return Task.FromResult((IModelClient<TInput, TOutput>)new RandomProductRecommender(productStore));
            }
            else
            {
                throw new NotImplementedException("This type of client is not creatable");
            }
        }
    }
}
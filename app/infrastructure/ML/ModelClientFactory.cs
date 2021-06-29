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

        public ModelClientFactory(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
            else
            {
                throw new NotImplementedException("That Hosting Type and Model Type is not supported.");
            }
        }
    }
}
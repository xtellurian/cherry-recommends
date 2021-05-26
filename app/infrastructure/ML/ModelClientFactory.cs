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

        public Task<IModelClient> GetClient(ModelRegistration model)
        {
            if (model.HostingType == HostingTypes.AzureMLContainerInstance && model.ModelType == ModelTypes.SingleClassClassifier)
            {
                return Task.FromResult<IModelClient>(new AzureMLClassifierClient(httpClient));
            }
            else
            {
                throw new NotImplementedException("That Hosting Type and Model Type is not supported.");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class ModelRegistrationWorkflows : IWorkflow
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly HttpClient httpClient;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IModelClientFactory modelClientFactory;

        public ModelRegistrationWorkflows(HttpClient httpClient,
                                          IModelRegistrationStore modelRegistrationStore,
                                          IModelClientFactory modelClientFactory)
        {
            this.httpClient = httpClient;
            this.modelRegistrationStore = modelRegistrationStore;
            this.modelClientFactory = modelClientFactory;
        }
        private ModelTypes ParseModelType(string modelType)
        {
            return ModelTypes.SingleClassClassifier; // there's only 1
        }

        private HostingTypes ParseHostingType(string hostingType)
        {
            return HostingTypes.AzureMLContainerInstance; // there's only 1
        }

        public async Task<ModelRegistration> RegisterNewModel(string name, Uri scoringUrl, Uri swaggerUri, string modelType, string hostingType, string key)
        {
            var modelTypeEnum = ParseModelType(modelType);
            var hostingTypeEnum = ParseHostingType(hostingType);
            // get the swagger definition
            var r = await httpClient.GetAsync(swaggerUri);
            r.EnsureSuccessStatusCode();
            var content = await r.Content.ReadAsStringAsync();
            var swagger = JsonSerializer.Deserialize<SwaggerDefinition>(content, serializerOptions);
            if (swagger.Swagger == null)
            {
                throw new ArgumentException("Swagger URL wasn't parsed correctly");
            }
            return await modelRegistrationStore.Create(new ModelRegistration(name,
                                                                             modelTypeEnum,
                                                                             hostingTypeEnum,
                                                                             scoringUrl.ToString(),
                                                                             key,
                                                                             swagger));
        }

        public async Task<EvaluationResult> InvokeModel(long id, IDictionary<string, object> features)
        {
            var model = await modelRegistrationStore.Read(id);
            if (model.ModelType == ModelTypes.SingleClassClassifier && model.HostingType == HostingTypes.AzureMLContainerInstance)
            {
                var client = await modelClientFactory.GetClient(model);
                return await client.Invoke(model, features);
            }
            else
            {
                throw new NotImplementedException("Those kind of models cannot be evaluated yet!");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class ModelRegistrationWorkflows : IWorkflow
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly HttpClient httpClient;
        private readonly ILogger<ModelRegistrationWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public ModelRegistrationWorkflows(HttpClient httpClient,
                                          ILogger<ModelRegistrationWorkflows> logger,
                                          IStorageContext storageContext,
                                          IModelRegistrationStore modelRegistrationStore)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.storageContext = storageContext;
            this.modelRegistrationStore = modelRegistrationStore;
        }
        private ModelTypes ParseModelType(string modelType)
        {
            return Enum.Parse<ModelTypes>(modelType);
        }

        private HostingTypes ParseHostingType(string hostingType)
        {
            return HostingTypes.AzureMLContainerInstance; // there's only 1
        }

        public async Task<ModelRegistration> RegisterNewModel(string name, Uri scoringUrl, string swaggerUri, string modelType, string hostingType, string key)
        {
            var modelTypeEnum = ParseModelType(modelType);
            var hostingTypeEnum = ParseHostingType(hostingType);
            // get the swagger definition, if it exists.
            SwaggerDefinition swagger;
            if (!string.IsNullOrEmpty(swaggerUri))
            {
                var r = await httpClient.GetAsync(swaggerUri);
                r.EnsureSuccessStatusCode();
                var content = await r.Content.ReadAsStringAsync();
                swagger = JsonSerializer.Deserialize<SwaggerDefinition>(content, serializerOptions);
                if (swagger.Swagger == null)
                {
                    throw new ArgumentException("Swagger URL wasn't parsed correctly");
                }
            }
            else
            {
                logger.LogWarning($"No Swagger provided for model: {name}");
                swagger = null;
            }
            var model = await modelRegistrationStore.Create(new ModelRegistration(name,
                                                                             modelTypeEnum,
                                                                             hostingTypeEnum,
                                                                             scoringUrl.ToString(),
                                                                             key,
                                                                             swagger));

            await storageContext.SaveChanges();
            return model;
        }
    }
}
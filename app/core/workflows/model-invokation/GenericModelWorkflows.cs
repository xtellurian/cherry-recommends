using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Workflows
{
    public class GenericModelWorkflows : IWorkflow
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly ILogger<GenericModelWorkflows> logger;
        private readonly HttpClient httpClient;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public GenericModelWorkflows(ILogger<GenericModelWorkflows> logger,
                                    HttpClient httpClient,
                                    IStorageContext storageContext,
                                    IModelRegistrationStore modelRegistrationStore)
        {
            this.logger = logger;
            this.httpClient = httpClient;
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

        public async Task<(string, HttpResponseMessage)> InvokeGeneric(long id, string input)
        {
            var model = await modelRegistrationStore.Read(id);
            var inputContent = new StringContent(input, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", model.Key);
            var response = await httpClient.PostAsync(model.ScoringUrl, inputContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            return (responseContent, response);
        }
    }
}
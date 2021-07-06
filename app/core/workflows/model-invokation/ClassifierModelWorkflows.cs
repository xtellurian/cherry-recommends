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
    public class ClassifierModelWorkflows : IWorkflow
    {
        private readonly ILogger<ClassifierModelWorkflows> logger;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IModelClientFactory modelClientFactory;

        public ClassifierModelWorkflows(ILogger<ClassifierModelWorkflows> logger,
                                    IModelRegistrationStore modelRegistrationStore,
                                    IModelClientFactory modelClientFactory)
        {
            this.logger = logger;
            this.modelRegistrationStore = modelRegistrationStore;
            this.modelClientFactory = modelClientFactory;
        }
        private ModelTypes ParseModelType(string modelType)
        {
            return Enum.Parse<ModelTypes>(modelType);
        }

        private HostingTypes ParseHostingType(string hostingType)
        {
            return HostingTypes.AzureMLContainerInstance; // there's only 1
        }

        public async Task<AzureMLClassifierOutput> InvokeClassifierModel(long id, string version, AzureMLModelInput input)
        {
            var model = await modelRegistrationStore.Read(id);
            input.Version ??= version ?? "default";
            if (model.HostingType == HostingTypes.AzureMLContainerInstance)
            {
                if (model.ModelType == ModelTypes.SingleClassClassifier)
                {
                    var client = await modelClientFactory
                        .GetClient<AzureMLModelInput, AzureMLClassifierOutput>(model);
                    return await client.Invoke(model, version, input);
                }
            }

            throw new NotImplementedException("Those kind of models cannot be evaluated yet!");

        }
    }
}
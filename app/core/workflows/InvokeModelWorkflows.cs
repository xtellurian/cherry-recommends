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
    public class InvokeModelWorkflows : IWorkflow
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly ILogger<InvokeModelWorkflows> logger;
        private readonly HttpClient httpClient;
        private readonly IStorageContext storageContext;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IModelClientFactory modelClientFactory;

        public InvokeModelWorkflows(ILogger<InvokeModelWorkflows> logger,
                                    HttpClient httpClient,
                                    IStorageContext storageContext,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IParameterSetRecommenderStore parameterSetRecommenderStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    IModelRegistrationStore modelRegistrationStore,
                                    IModelClientFactory modelClientFactory)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.storageContext = storageContext;
            this.correlatorStore = correlatorStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
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

        public async Task<(string, HttpResponseMessage)> InvokeGeneric(long id, string input)
        {
            var model = await modelRegistrationStore.Read(id);
            var inputContent = new StringContent(input, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", model.Key);
            var response = await httpClient.PostAsync(model.ScoringUrl, inputContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            return (responseContent, response);
        }

        public async Task<ParameterSetRecommenderModelOutputV1> InvokeParameterSetRecommender(long id, string version, ParameterSetRecommenderModelInputV1 input)
        {
            var model = await modelRegistrationStore.Read(id);
            if (model.ModelType != ModelTypes.ParameterSetRecommenderV1)
            {
                throw new BadRequestException("Model is not a ParameterSetRecommenderV1");
            }
            if (!input.ParameterSetRecommenderId.HasValue)
            {
                throw new BadRequestException("ParameterSetRecommenderId is a required parameter.");
            }
            var recommender = await parameterSetRecommenderStore.Read(input.ParameterSetRecommenderId.Value);
            // enrich with the parameter bounds if not supplied.
            if (input.ParameterBounds == null || input.ParameterBounds.Count == 0)
            {
                logger.LogInformation("Updating parameter bounds for invoke request");
                input.ParameterBounds = recommender.ParameterBounds;
            }
            // check all arguments are supplied, and add defaults if necessary
            foreach (var r in recommender.Arguments)
            {
                if (!input.Arguments.ContainsKey(r.CommonId))
                {
                    if (r.IsRequired)
                    {
                        throw new BadRequestException($"The argument {r.CommonId} is required.");
                    }
                    else
                    {
                        input.Arguments[r.CommonId] = r.DefaultValue;
                    }
                }
            }

            var client = await modelClientFactory
                .GetClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1>(model);
            var output = await client.Invoke(model, version, input);

            // now save the result
            var correlation = await correlatorStore.Create(new RecommendationCorrelator());
            var recommendation = new ParameterSetRecommendation(correlation, version);
            recommendation.SetInput(input);
            recommendation.SetOutput(output);
            recommendation = await parameterSetRecommendationStore.Create(recommendation);

            await storageContext.SaveChanges();
            return output;
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
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
    public class ParameterSetRecommenderModelWorkflows : IWorkflow
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly ILogger<ParameterSetRecommenderModelWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly IModelClientFactory modelClientFactory;

        public ParameterSetRecommenderModelWorkflows(ILogger<ParameterSetRecommenderModelWorkflows> logger,
                                    IStorageContext storageContext,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IParameterSetRecommenderStore parameterSetRecommenderStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    IModelRegistrationStore modelRegistrationStore,
                                    ITrackedUserStore trackedUserStore,
                                    IModelClientFactory modelClientFactory)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.correlatorStore = correlatorStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.modelRegistrationStore = modelRegistrationStore;
            this.trackedUserStore = trackedUserStore;
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

        public async Task<ParameterSetRecommenderModelOutputV1> InvokeParameterSetRecommender(long id, string version, ParameterSetRecommenderModelInputV1 input)
        {
            var recommender = await parameterSetRecommenderStore.Read(id, _ => _.ModelRegistration);
            var model = recommender.ModelRegistration;
            TrackedUser user = null;
            if (!string.IsNullOrEmpty(input.CommonUserId))
            {
                user = await trackedUserStore.ReadFromCommonId(input.CommonUserId);
            }
            if (model == null)
            {
                throw new ConfigurationException($"Parameter Set Recommender {recommender.Id} has no attached model");
            }
            if (model.ModelType != ModelTypes.ParameterSetRecommenderV1)
            {
                throw new BadRequestException("Model is not a ParameterSetRecommenderV1");
            }
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
            var correlator = await correlatorStore.Create(new RecommendationCorrelator());
            var recommendation = new ParameterSetRecommendation(recommender, user, correlator, version);
            recommendation.SetInput(input);
            recommendation.SetOutput(output);
            recommendation = await parameterSetRecommendationStore.Create(recommendation);

            await storageContext.SaveChanges();

            output.CorrelatorId = correlator.Id;
            return output;
        }
    }
}
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public class ParameterSetRecommenderInvokationWorkflows : RecommenderInvokationWorkflowBase<ParameterSetRecommender>, IWorkflow
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly ILogger<ParameterSetRecommenderInvokationWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly IRecommenderModelClientFactory modelClientFactory;

        public ParameterSetRecommenderInvokationWorkflows(ILogger<ParameterSetRecommenderInvokationWorkflows> logger,
                                    IStorageContext storageContext,
                                    IDateTimeProvider dateTimeProvider,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IParameterSetRecommenderStore parameterSetRecommenderStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    IModelRegistrationStore modelRegistrationStore,
                                    ITrackedUserStore trackedUserStore,
                                    IRecommenderModelClientFactory modelClientFactory)
                                     : base(storageContext, parameterSetRecommenderStore, dateTimeProvider)
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

        public async Task<ParameterSetRecommenderModelOutputV1> InvokeParameterSetRecommender(long id, string version, ParameterSetRecommenderModelInputV1 input)
        {
            var recommender = await parameterSetRecommenderStore.Read(id);
            var invokationEntry = await base.StartTrackInvokation(recommender);
            TrackedUser user = null;
            try
            {
                var model = recommender.ModelRegistration;
                user = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");

                IRecommenderModelClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1> client;
                if (model == null)
                {
                    // create a random recommender here.
                    client = await modelClientFactory.GetUnregisteredClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1>(recommender);
                    logger.LogWarning($"Using unregistered model client for {recommender.Id}");
                }
                else if (model.ModelType != ModelTypes.ParameterSetRecommenderV1)
                {
                    throw new BadRequestException("Model is not a ParameterSetRecommenderV1");
                }
                else
                {
                    client = await modelClientFactory
                        .GetClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1>(recommender);
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

                // invoke the model
                var output = await client.Invoke(recommender, version, input);

                // now save the result
                var correlator = await correlatorStore.Create(new RecommendationCorrelator());
                var recommendation = new ParameterSetRecommendation(recommender, user, correlator, version);
                recommendation.SetInput(input);
                recommendation.SetOutput(output);
                recommendation = await parameterSetRecommendationStore.Create(recommendation);

                await storageContext.SaveChanges();

                output.CorrelatorId = correlator.Id;

                await base.EndTrackInvokation(invokationEntry,
                                              true,
                                              user,
                                              correlator,
                                              $"Invoked successfully for {user.Name ?? user.CommonId}",
                                              null,
                                              true);
                return output;
            }
            catch (ModelInvokationException modelEx)
            {
                logger.LogError("Error invoking recommender", modelEx);
                await base.EndTrackInvokation(invokationEntry, false, user, null, $"Invoke failed for {user?.Name ?? user?.CommonId}", modelEx.ModelResponseContent, true);
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error invoking recommender", ex);
                await base.EndTrackInvokation(invokationEntry, false, user, null, $"Invoke failed for {user?.Name ?? user?.CommonId}", null, true);
            }

            try
            {
                await parameterSetRecommenderStore.LoadMany(recommender, _ => _.Parameters);
                var recommendedParams = new Dictionary<string, object>();
                foreach (var p in recommender.Parameters)
                {
                    recommendedParams[p.CommonId] = p.DefaultValue?.Value;
                }

                return new ParameterSetRecommenderModelOutputV1
                {
                    RecommendedParameters = recommendedParams
                };
            }
            catch (System.Exception ex)
            {
                logger.LogCritical($"Failed to return default parameters for parameterset recommender {recommender.Id}", ex);
                throw;
            }
        }
    }
}
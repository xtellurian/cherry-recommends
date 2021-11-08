using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

#nullable enable
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
        private readonly IRecommendationCache<ParameterSetRecommender, ParameterSetRecommendation> recommendationCache;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly IRecommenderModelClientFactory modelClientFactory;

        public ParameterSetRecommenderInvokationWorkflows(ILogger<ParameterSetRecommenderInvokationWorkflows> logger,
                                    IStorageContext storageContext,
                                    IDateTimeProvider dateTimeProvider,
                                    IRecommendationCache<ParameterSetRecommender, ParameterSetRecommendation> recommendationCache,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IParameterSetRecommenderStore parameterSetRecommenderStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    IModelRegistrationStore modelRegistrationStore,
                                    IWebhookSenderClient webhookSenderClient,
                                    IHistoricTrackedUserFeatureStore historicFeatureStore,
                                    ITrackedUserStore trackedUserStore,
                                    IRecommenderModelClientFactory modelClientFactory)
                                     : base(storageContext, parameterSetRecommenderStore, historicFeatureStore, webhookSenderClient, dateTimeProvider)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.recommendationCache = recommendationCache;
            this.correlatorStore = correlatorStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.modelRegistrationStore = modelRegistrationStore;
            this.trackedUserStore = trackedUserStore;
            this.modelClientFactory = modelClientFactory;
        }

        public async Task<ParameterSetRecommendation> InvokeParameterSetRecommender(
            ParameterSetRecommender recommender,
            ParameterSetRecommenderModelInputV1 input,
            string? trigger = null)
        {
            // use the correlator to begin with because need to pass an event ID into some models
            await parameterSetRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            await parameterSetRecommenderStore.LoadMany(recommender, _ => _.Parameters);
            var invokeLog = await base.StartTrackInvokation(recommender, input);
            RecommendingContext context = new RecommendingContext(invokeLog, trigger);

            try
            {
                var model = recommender.ModelRegistration;
                if (input.CommonUserId == null)
                {
                    throw new BadRequestException("CommonUserId must be provided when invoking recommender");
                }
                context.TrackedUser = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");

                // check the cache and load the context 
                if (await recommendationCache.HasCached(recommender, context.TrackedUser))
                {
                    var rec = await recommendationCache.GetCached(recommender, context.TrackedUser);
                    await parameterSetRecommendationStore.Load(rec, _ => _.RecommendationCorrelator);
                    context.Correlator = rec.RecommendationCorrelator;
                    await base.EndTrackInvokation(context, true, "Completed using cached recommendation");
                }
                else
                {
                    context.Correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
                    logger.LogInformation("Saving correlator to create Id");
                    await storageContext.SaveChanges();
                }

                // load the features from the user
                input.Features = await base.GetFeatures(context);

                IRecommenderModelClient<ParameterSetRecommenderModelOutputV1> client;
                if (model == null)
                {
                    // create a random recommender here.
                    client = await modelClientFactory.GetUnregisteredClient<ParameterSetRecommenderModelOutputV1>(recommender);
                    logger.LogWarning($"Using unregistered model client for {recommender.Id}");
                }
                else if (model.ModelType != ModelTypes.ParameterSetRecommenderV1)
                {
                    throw new BadRequestException("Model is not a ParameterSetRecommenderV1");
                }
                else
                {
                    context.Correlator.ModelRegistration = recommender.ModelRegistration;
                    client = await modelClientFactory
                        .GetClient<ParameterSetRecommenderModelOutputV1>(recommender);
                }
                // enrich with the parameter bounds if not supplied.
                if (input.ParameterBounds == null || input.ParameterBounds.Count() == 0)
                {
                    logger.LogInformation("Updating parameter bounds for invoke request");
                    input.ParameterBounds = recommender.ParameterBounds;
                }
                // check all arguments are supplied, and add defaults if necessary
                if (recommender.Arguments != null)
                {
                    foreach (var r in recommender.Arguments)
                    {
                        CheckArgument(recommender, r, input, context);
                    }
                }

                // invoke the model
                var output = await client.Invoke(recommender, context, input);


                var recommendation = new ParameterSetRecommendation(recommender, context);
                recommendation.SetInput(input);
                recommendation.SetOutput(output);
                recommendation = await parameterSetRecommendationStore.Create(recommendation);
                await storageContext.SaveChanges();

                output.CorrelatorId = context.Correlator.Id;

                context.LogMessage("Created a recommendation entity");

                // send to any destinations
                await base.SendToDestinations(recommender, context, recommendation);


                await base.EndTrackInvokation(context,
                                              true,
                                              $"Invoked successfully for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId}",
                                              saveOnComplete: true);
                return recommendation;
            }
            catch (ModelInvokationException modelEx)
            {
                logger.LogError("Error invoking recommender", modelEx);
                await base.EndTrackInvokation(context, false, message: $"Invoke failed for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId ?? input.CommonUserId}", modelEx.ModelResponseContent, saveOnComplete: true);
                if (recommender.ShouldThrowOnBadInput())
                {
                    throw;
                }
                else
                {
                    context.InvokationLog.LogMessage("There was an error, but the recommender is not set to throw.");
                    logger.LogError("Model invokation Error", modelEx);
                }
            }
            catch (BadRequestException badReqEx)
            {
                logger.LogError("Error invoking recommender", badReqEx);
                await base.EndTrackInvokation(context, false,
                    message: $"Panic! Bad Request: {badReqEx.Title}",
                    saveOnComplete: true);
                throw;
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error invoking recommender", ex);
                await base.EndTrackInvokation(context, false,
                    message: $"Invoke failed for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId ?? input.CommonUserId}",
                    saveOnComplete: true);
                if (recommender.ShouldThrowOnBadInput())
                {
                    throw;
                }
                else
                {
                    context.InvokationLog.LogMessage("There was an error, but the recommender is not set to throw.");
                    logger.LogError("Exception during Invokation.", ex);
                }
            }

            try
            {
                await parameterSetRecommenderStore.LoadMany(recommender, _ => _.Parameters);
                var recommendedParams = new Dictionary<string, object?>();
                foreach (var p in recommender.Parameters)
                {
                    recommendedParams[p.CommonId] = p.DefaultValue?.Value;
                }

                var recommendation = new ParameterSetRecommendation(recommender, context);
                return recommendation;
            }
            catch (System.Exception ex)
            {
                logger.LogCritical($"Failed to return default parameters for parameterset recommender {recommender.Id}", ex);
                throw;
            }
            finally
            {
                await base.EndTrackInvokation(context,
                                              false,
                                              message: $"Invoke failed for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId ?? input.CommonUserId}",
                                              modelResponse: null,
                                              saveOnComplete: true);
            }
        }
    }
}
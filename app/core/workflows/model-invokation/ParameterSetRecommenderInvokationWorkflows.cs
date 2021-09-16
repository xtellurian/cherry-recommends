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
                                    IHistoricTrackedUserFeatureStore historicFeatureStore,
                                    ITrackedUserStore trackedUserStore,
                                    IRecommenderModelClientFactory modelClientFactory)
                                     : base(storageContext, parameterSetRecommenderStore, historicFeatureStore, dateTimeProvider)
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

        public async Task<ParameterSetRecommendation> InvokeParameterSetRecommender(
            ParameterSetRecommender recommender,
            string version,
            ParameterSetRecommenderModelInputV1 input)
        {
            // use the correlator to begin with because need to pass an event ID into some models
            await parameterSetRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            var correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
            var invokationEntry = await base.StartTrackInvokation(recommender, input, saveOnComplete: false);
            await storageContext.SaveChanges(); // save the correlator and invokation entry
            var recommendingContext = new RecommendingContext(version, correlator);
            TrackedUser user = null;
            try
            {
                var model = recommender.ModelRegistration;
                user = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");

                // load the features from the user
                input.Features = await base.GetFeatures(user, invokationEntry);

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
                    correlator.ModelRegistration = recommender.ModelRegistration;
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
                    CheckArgument(recommender, r, input, invokationEntry);
                }

                // invoke the model
                var output = await client.Invoke(recommender, recommendingContext, input);


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
                return recommendation;
            }
            catch (ModelInvokationException modelEx)
            {
                logger.LogError("Error invoking recommender", modelEx);
                await base.EndTrackInvokation(invokationEntry, false, user, null, $"Invoke failed for {user?.Name ?? user?.CommonId}", modelEx.ModelResponseContent, saveOnComplete: true);
                if (recommender.ShouldThrowOnBadInput())
                {
                    throw;
                }
                else
                {
                    invokationEntry.LogMessage("There was an error, but the recommender is not set to throw.");
                    logger.LogError("Model invokation Error", modelEx);
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error invoking recommender", ex);
                await base.EndTrackInvokation(invokationEntry, false, user, null, $"Invoke failed for {user?.Name ?? user?.CommonId}", null, saveOnComplete: true);
                if (recommender.ShouldThrowOnBadInput())
                {
                    throw;
                }
                else
                {
                    invokationEntry.LogMessage("There was an error, but the recommender is not set to throw.");
                    logger.LogError("Exception during Invokation.", ex);
                }
            }

            try
            {
                await parameterSetRecommenderStore.LoadMany(recommender, _ => _.Parameters);
                var recommendedParams = new Dictionary<string, object>();
                foreach (var p in recommender.Parameters)
                {
                    recommendedParams[p.CommonId] = p.DefaultValue?.Value;
                }

                var recommendation = new ParameterSetRecommendation(recommender, user, correlator, version);
                return recommendation;
            }
            catch (System.Exception ex)
            {
                logger.LogCritical($"Failed to return default parameters for parameterset recommender {recommender.Id}", ex);
                throw;
            }
            finally
            {
                await base.EndTrackInvokation(invokationEntry,
                                              false,
                                              user,
                                              null,
                                              message: $"Invoke failed for {user?.Name ?? user?.CommonId}",
                                              modelResponse: null,
                                              saveOnComplete: true);
            }
        }
        /// <summary>
        /// It meant to throw in some situations.
        /// </summary>
        private void CheckArgument(ParameterSetRecommender recommender,
                                   RecommenderArgument arg,
                                   ParameterSetRecommenderModelInputV1 input,
                                   InvokationLogEntry invokationEntry)
        {
            input.Arguments ??= new Dictionary<string, object>(); // ensure no null refs here
            if (!input.Arguments.ContainsKey(arg.CommonId))
            {
                // argument is missing
                if (arg.IsRequired && recommender.ShouldThrowOnBadInput())
                {
                    throw new BadRequestException("Missing recommender argument",
                        $"The argument {arg.CommonId} is required, and the recommender is set to throw on errors.");
                }
                else
                {
                    invokationEntry.LogMessage($"Using default value ({arg.DefaultArgumentValue}) for argument {arg.CommonId}");
                    input.Arguments[arg.CommonId] = arg.DefaultArgumentValue;
                }
            }
            else
            {
                // incoming argument exists. check the type.
                var val = input.Arguments[arg.CommonId]?.ToString();
                if (val == null && arg.IsRequired && recommender.ShouldThrowOnBadInput())
                {
                    throw new BadRequestException("Null recommender argument",
                        $"The argument {arg.CommonId} is null, and the recommender is set to throw on errors.");
                }
                else if (arg.ArgumentType == ArgumentTypes.Numerical)
                {
                    // try and parse as a number
                    if (!double.TryParse(val, out _))
                    {
                        // the value was bad.
                        if (recommender.ShouldThrowOnBadInput())
                        {
                            invokationEntry.LogMessage($"The argument {arg.CommonId} should be numeric, and the recommender is set to throw on errors.");
                            throw new BadRequestException("Bad recommender argument",
                                $"The argument {arg.CommonId} should be numeric, and the recommender is set to throw on errors.");
                        }
                        else
                        {
                            // try and set the value to the default
                            invokationEntry.LogMessage($"Using default value ({arg.DefaultArgumentValue}) for argument {arg.CommonId}");
                            input.Arguments[arg.CommonId] = arg.DefaultArgumentValue;
                        }
                    }
                }
                else
                {
                    invokationEntry.LogMessage($"Categorical arguments are not validated");
                }
            }
        }
    }
}
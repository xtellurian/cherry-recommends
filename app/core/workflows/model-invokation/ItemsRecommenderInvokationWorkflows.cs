using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ItemsRecommenderInvokationWorkflows : RecommenderInvokationWorkflowBase<ItemsRecommender>, IWorkflow
    {
        private readonly ILogger<ItemsRecommenderInvokationWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IRecommendationCache<ItemsRecommender, ItemsRecommendation> recommendationCache;
        private readonly IRecommenderModelClientFactory modelClientFactory;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly IRecommendableItemStore itemStore;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly IItemsRecommendationStore itemsRecommendationStore;

        public ItemsRecommenderInvokationWorkflows(ILogger<ItemsRecommenderInvokationWorkflows> logger,
                                    IStorageContext storageContext,
                                    IDateTimeProvider dateTimeProvider,
                                    IRecommendationCache<ItemsRecommender, ItemsRecommendation> recommendationCache,
                                    IRecommenderModelClientFactory modelClientFactory,
                                    ITrackedUserStore trackedUserStore,
                                    ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                    IHistoricTrackedUserFeatureStore historicFeatureStore,
                                    IRecommendableItemStore itemStore,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IItemsRecommenderStore itemsRecommenderStore,
                                    IItemsRecommendationStore itemsRecommendationStore)
                                     : base(storageContext, itemsRecommenderStore, historicFeatureStore, dateTimeProvider)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.recommendationCache = recommendationCache;
            this.modelClientFactory = modelClientFactory;
            this.trackedUserStore = trackedUserStore;
            this.itemStore = itemStore;
            this.correlatorStore = correlatorStore;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.itemsRecommendationStore = itemsRecommendationStore;
        }

        public async Task<ItemsRecommendation> InvokeItemsRecommender(
            ItemsRecommender recommender,
            IItemsModelInput input)
        {
            await itemsRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            var invokationEntry = await base.StartTrackInvokation(recommender, input);
            RecommendingContext context = new RecommendingContext(invokationEntry);
            var userName = input.CommonUserId;
            try
            {
                if (string.IsNullOrEmpty(input.CommonUserId))
                {
                    throw new BadRequestException("ParameterSetRecommenderId is a required parameter.");
                }

                // enrich values from the touchpoint
                context.TrackedUser = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");
                userName = context.TrackedUser.Name ?? context.TrackedUser.CommonId ?? userName;
                // check the cache
                if (await recommendationCache.HasCached(recommender, context.TrackedUser))
                {
                    var rec = await recommendationCache.GetCached(recommender, context.TrackedUser);
                    // ensure to load the items
                    await itemsRecommendationStore.LoadMany(rec, _ => _.Items);
                    await itemsRecommendationStore.Load(rec, _ => _.RecommendationCorrelator);
                    context.Correlator = rec.RecommendationCorrelator;
                    await base.EndTrackInvokation(context, true, "Completed using cached recommendation");
                    return rec;
                }
                else
                {
                    context.Correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
                    logger.LogInformation("Saving correlator to create Id");
                    await storageContext.SaveChanges();
                }


                // load the features of the tracked user
                input.Features = await base.GetFeatures(context);

                // load the items that a model can choose from
                await itemsRecommenderStore.LoadMany(recommender, _ => _.Items);
                if (recommender.Items.Any())
                {
                    input.Items = recommender.Items;
                    logger.LogInformation($"Using {input.Items.Count()} items");
                    invokationEntry.LogMessage($"Using {input.Items.Count()} items");
                }
                else
                {
                    var allItems = await itemStore.Query(1); // this is a fudge, it will only be the top items
                    input.Items = allItems.Items;
                    logger.LogInformation($"Using all items as input, {input.Items.Count()} items");
                    invokationEntry.LogMessage($"Using all items as input, {input.Items.Count()} items");
                }

                IRecommenderModelClient<ItemsRecommenderModelOutputV1> client;
                if (recommender.ModelRegistration == null)
                {
                    // load the items required for the random recommender
                    await itemsRecommenderStore.LoadMany(recommender, _ => _.Items);
                    // create a random recommender here.
                    client = await modelClientFactory.GetUnregisteredItemsRecommenderClient<ItemsRecommenderModelOutputV1>(recommender);
                    logger.LogWarning($"Using unregistered model client for {recommender.Id}");
                }
                else if (recommender.ModelRegistration.ModelType != ModelTypes.ItemsRecommenderV1)
                {
                    throw new BadRequestException("Model is not a ItemsRecommender");
                }
                else
                {
                    context.Correlator.ModelRegistration = recommender.ModelRegistration;
                    client = await modelClientFactory
                       .GetClient<ItemsRecommenderModelOutputV1>(recommender);
                }

                // check all arguments are supplied, and add defaults if necessary
                if (recommender.Arguments != null)
                {
                    foreach (var r in recommender.Arguments)
                    {
                        CheckArgument(recommender, r, input, context);
                    }
                }

                var output = await client.Invoke(recommender, context, input);

                // load the items
                foreach (var scoredItem in output.ScoredItems)
                {
                    if (scoredItem == null)
                    {
                        throw new ModelInvokationException("Model returned a null ScoredItem");
                    }
                    else if (scoredItem.ItemId.HasValue)
                    {
                        scoredItem.Item ??= await itemStore.Read(scoredItem.ItemId.Value);
                    }
                    else if (!string.IsNullOrEmpty(scoredItem.ItemCommonId))
                    {
                        scoredItem.Item ??= await itemStore.ReadFromCommonId(scoredItem.ItemCommonId);
                    }
                    else if (scoredItem.Item == null)
                    {
                        throw new ModelInvokationException("The model did not return a valid item.");
                    }
                }

                return await HandleRecommendation(recommender, context, input, output);
            }
            catch (CommonIdException commonIdEx)
            {
                await base.EndTrackInvokation(
                    context,
                    false,
                    message: $"Invoke failed for {userName}",
                    modelResponse: commonIdEx.Message);
                throw; // rethrow the error to propagate to calling client
            }
            catch (System.Exception ex)
            {
                string? modelResponseContent = null;
                if (ex is ModelInvokationException modelEx)
                {
                    logger.LogError("Error invoking recommender", modelEx);
                    modelResponseContent = modelEx.ModelResponseContent;
                }
                else
                {
                    logger.LogError("Error invoking recommender model");
                }

                if (recommender.ShouldThrowOnBadInput())
                {
                    await base.EndTrackInvokation(
                        context,
                        false,
                        message: $"Invoke failed for {userName}",
                        modelResponse: modelResponseContent,
                        saveOnComplete: true);
                    throw; // rethrow the error to propagate to calling client
                }
                else if (recommender.DefaultItemId != null)
                {
                    // case: default item and the model returned error
                    await itemsRecommenderStore.Load(recommender, _ => _.DefaultItem);
                    invokationEntry.LogMessage($"Model Error. Fallback to default item");
                    var output = new ItemsRecommenderModelOutputV1
                    {
                        ScoredItems = new List<ScoredRecommendableItem> { new ScoredRecommendableItem(recommender.DefaultItem!, 0) }
                    };
                    return await HandleRecommendation(recommender, context, input, output);
                }
                else
                {
                    // case: no default and the model returned error
                    var someItems = await itemStore.Query(1);
                    var item = someItems.Items.First();
                    invokationEntry.LogMessage($"Model Error. Fallback to top item {item.CommonId}");
                    var output = new ItemsRecommenderModelOutputV1
                    {
                        ScoredItems = new List<ScoredRecommendableItem> { new ScoredRecommendableItem(item, 0) }
                    };
                    return await HandleRecommendation(recommender, context, input, output);
                }
            }
        }

        private async Task<ItemsRecommendation> HandleRecommendation(ItemsRecommender recommender,
                                                                        RecommendingContext context,
                                                                        IModelInput input,
                                                                        ItemsRecommenderModelOutputV1 output)
        {
            // now save the result

            var recommendation = new ItemsRecommendation(recommender, context.TrackedUser, context.Correlator, output.ScoredItems);
            output.CorrelatorId = context.Correlator?.Id;
            recommendation.SetInput(input);
            recommendation.SetOutput(output);

            recommendation = await itemsRecommendationStore.Create(recommendation);
            await base.EndTrackInvokation(context,
                                          true,
                                          message: $"Invoked successfully for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId}",
                                          modelResponse: null,
                                          saveOnComplete: true);

            // set this after the context has been saved.
            output.CorrelatorId = context.Correlator?.Id;
            return recommendation;
        }
    }
}
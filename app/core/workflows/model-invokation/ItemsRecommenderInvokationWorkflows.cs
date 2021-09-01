using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public class ItemsRecommenderInvokationWorkflows : RecommenderInvokationWorkflowBase<ItemsRecommender>, IWorkflow
    {
        private readonly ILogger<ItemsRecommenderInvokationWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IRecommenderModelClientFactory modelClientFactory;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly IRecommendableItemStore itemStore;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly IItemsRecommendationStore itemsRecommendationStore;

        public ItemsRecommenderInvokationWorkflows(ILogger<ItemsRecommenderInvokationWorkflows> logger,
                                    IStorageContext storageContext,
                                    IDateTimeProvider dateTimeProvider,
                                    IRecommenderModelClientFactory modelClientFactory,
                                    ITrackedUserStore trackedUserStore,
                                    ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                    ITrackedUserFeatureStore featureStore,
                                    IRecommendableItemStore itemStore,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IItemsRecommenderStore itemsRecommenderStore,
                                    IItemsRecommendationStore itemsRecommendationStore)
                                     : base(storageContext, itemsRecommenderStore, featureStore, dateTimeProvider)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.modelClientFactory = modelClientFactory;
            this.trackedUserStore = trackedUserStore;
            this.itemStore = itemStore;
            this.correlatorStore = correlatorStore;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.itemsRecommendationStore = itemsRecommendationStore;
        }

        public async Task<ItemsRecommendation> InvokeItemsRecommender(
            ItemsRecommender recommender,
            IModelInput input)
        {
            await itemsRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            var invokationEntry = await base.StartTrackInvokation(recommender, input, saveOnComplete: false);
            var correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
            await storageContext.SaveChanges(); // save the correlator and invokatin entry

            var recommendingContext = new RecommendingContext(correlator);
            TrackedUser user = null;
            try
            {
                if (string.IsNullOrEmpty(input.CommonUserId))
                {
                    throw new BadRequestException("ParameterSetRecommenderId is a required parameter.");
                }

                // enrich values from the touchpoint
                user = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");

                // load the features of the tracked user
                input.Features = await base.GetFeatures(user, invokationEntry);

                IRecommenderModelClient<IModelInput, ItemsRecommenderModelOutputV1> client;
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
                    correlator.ModelRegistration = recommender.ModelRegistration;
                    client = await modelClientFactory
                       .GetClient<IModelInput, ItemsRecommenderModelOutputV1>(recommender);
                }

                var output = await client.Invoke(recommender, recommendingContext, input);

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

                return await HandleRecommendation(recommender, recommendingContext, input, invokationEntry, user, output);
            }
            catch (System.Exception ex)
            {
                string modelResponseContent = null;
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
                        invokationEntry,
                        false,
                        user,
                        null,
                        $"Invoke failed for {user?.Name ?? user?.CommonId}",
                        modelResponseContent,
                        true);
                    throw; // rethrow the error to propagate to calling client
                }
                else if (recommender.DefaultItemId != null)
                {
                    // case: default item and the model returned error
                    await itemsRecommenderStore.Load(recommender, _ => _.DefaultItem);
                    invokationEntry.LogMessage($"Model Error. Fallback to default item");
                    var output = new ItemsRecommenderModelOutputV1
                    {
                        ScoredItems = new List<ScoredRecommendableItem> { new ScoredRecommendableItem(recommender.DefaultItem, 0) }
                    };
                    return await HandleRecommendation(recommender, recommendingContext, input, invokationEntry, user, output);
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
                    return await HandleRecommendation(recommender, recommendingContext, input, invokationEntry, user, output);
                }
            }
        }

        private async Task<ItemsRecommendation> HandleRecommendation(ItemsRecommender recommender,
                                                                        RecommendingContext recommendingContext,
                                                                        IModelInput input,
                                                                        InvokationLogEntry invokationEntry,
                                                                        TrackedUser user,
                                                                        ItemsRecommenderModelOutputV1 output)
        {
            // now save the result

            var recommendation = new ItemsRecommendation(recommender, user, recommendingContext.Correlator, output.ScoredItems);
            output.CorrelatorId = recommendingContext.Correlator.Id;
            recommendation.SetInput(input);
            recommendation.SetOutput(output);

            recommendation = await itemsRecommendationStore.Create(recommendation);
            await base.EndTrackInvokation(invokationEntry,
                                          true,
                                          user,
                                          recommendingContext.Correlator,
                                          $"Invoked successfully for {user.Name ?? user.CommonId}",
                                          null,
                                          false);

            await storageContext.SaveChanges();

            // set this after the context has been saved.
            output.CorrelatorId = recommendingContext.Correlator.Id;
            return recommendation;
        }
    }
}
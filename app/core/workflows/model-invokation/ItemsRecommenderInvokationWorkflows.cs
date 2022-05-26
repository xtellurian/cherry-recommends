using System;
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
        private readonly IRecommendationCache<ItemsRecommender, ItemsRecommendation> recommendationCache;
        private readonly IRecommenderModelClientFactory modelClientFactory;
        private readonly ICustomerWorkflow customerWorkflow;
        private readonly IBusinessWorkflow businessWorkflow;
        private readonly IRecommendableItemStore itemStore;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly IItemsRecommendationStore itemsRecommendationStore;
        private readonly IAudienceStore audienceStore;
        private readonly IInternalOptimiserClientFactory optimiserClientFactory;
        private readonly IDiscountCodeWorkflow discountCodeWorkflow;
        private readonly IOfferStore offerStore;

        public ItemsRecommenderInvokationWorkflows(ILogger<ItemsRecommenderInvokationWorkflows> logger,
                                    IDateTimeProvider dateTimeProvider,
                                    IRecommendationCache<ItemsRecommender, ItemsRecommendation> recommendationCache,
                                    IRecommenderModelClientFactory modelClientFactory,
                                    ICustomerWorkflow customerWorkflow,
                                    IBusinessWorkflow businessWorkflow,
                                    IStoreCollection storeCollection,
                                    IItemsRecommenderStore itemsRecommenderStore,
                                    IWebhookSenderClient webhookSenderClient,
                                    IInternalOptimiserClientFactory optimiserClientFactory,
                                    IDiscountCodeWorkflow discountCodeWorkflow,
                                    IKlaviyoSystemWorkflow klaviyoWorkflow)
                                    : base(itemsRecommenderStore, storeCollection, webhookSenderClient, dateTimeProvider, klaviyoWorkflow)
        {
            this.logger = logger;
            this.recommendationCache = recommendationCache;
            this.modelClientFactory = modelClientFactory;
            this.customerWorkflow = customerWorkflow;
            this.businessWorkflow = businessWorkflow;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.itemStore = storeCollection.ResolveStore<IRecommendableItemStore, RecommendableItem>();
            this.correlatorStore = storeCollection.ResolveStore<IRecommendationCorrelatorStore, RecommendationCorrelator>();
            this.itemsRecommendationStore = storeCollection.ResolveStore<IItemsRecommendationStore, ItemsRecommendation>();
            this.audienceStore = storeCollection.ResolveStore<IAudienceStore, Audience>();
            this.offerStore = storeCollection.ResolveStore<IOfferStore, Offer>();
            this.optimiserClientFactory = optimiserClientFactory;
            this.discountCodeWorkflow = discountCodeWorkflow;
        }

        public async Task<ItemsRecommendation> InvokeItemsRecommender(
            ItemsRecommender recommender,
            IItemsModelInput input,
            string? trigger = null)
        {
            await ThrowIfDisabled(recommender);
            switch (recommender.TargetType)
            {
                case PromotionRecommenderTargetTypes.Customer:
                    if (input.CustomerId == null)
                    {
                        throw new BadRequestException("CustomerId is required to invoke a recommender targeting Customer");
                    }
                    break;
                case PromotionRecommenderTargetTypes.Business:
                    if (input.BusinessId == null)
                    {
                        throw new BadRequestException("BusinessId is required to invoke a recommender targeting Business");
                    }
                    break;
            }

            await itemsRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            var invokationEntry = await base.StartTrackInvokation(recommender, input);
            var context = new RecommendingContext(invokationEntry, input, trigger);
            context.SetLogger(logger);

            string commonIdOrName;
            try
            {
                string commonId;
                if (recommender.TargetType == PromotionRecommenderTargetTypes.Customer)
                {
                    commonId = input.GetCustomerId()!; // initialise the name

                    context.Customer = await customerWorkflow.CreateOrUpdate(
                        new PendingCustomer(commonId, recommender.EnvironmentId, $"Auto-created by Recommender {recommender.Name}", false));
                    commonIdOrName = context.Customer.Name ?? context.Customer.CommonId ?? commonId;

                    // check the cache
                    if (await recommendationCache.HasCached(recommender, context.Customer))
                    {
                        var rec = await recommendationCache.GetCached(recommender, context.Customer);
                        // ensure to load the items
                        await itemsRecommendationStore.LoadMany(rec, _ => _.Items);
                        await itemsRecommendationStore.Load(rec, _ => _.RecommendationCorrelator);
                        await itemsRecommendationStore.LoadMany(rec, _ => _.DiscountCodes);
                        await discountCodeWorkflow.LoadGeneratedAt(rec.DiscountCodes);
                        context.Correlator = rec.RecommendationCorrelator;
                        await base.EndTrackInvokation(context, true, "Completed using cached recommendation");
                        return rec;
                    }
                    else
                    {
                        var audience = await audienceStore.GetAudience(recommender);
                        if (audience.Success && !await audienceStore.IsCustomerInAudience(context.Customer, audience.Entity))
                        {
                            throw new BadRequestException("Customer is not in recommender audience.");
                        }

                        context.Correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
                        logger.LogInformation("Saving correlator to create Id");
                        await correlatorStore.Context.SaveChanges();
                    }
                }
                else if (recommender.TargetType == PromotionRecommenderTargetTypes.Business && input.BusinessId != null)
                {
                    commonId = input.BusinessId; // initialise the name
                    context.Business = await businessWorkflow.CreateOrUpdate(
                        new PendingBusiness(input.BusinessId, recommender.EnvironmentId, $"Auto-created by Recommender {recommender.Name}", false));
                    commonIdOrName = context.Business.Name ?? context.Business.CommonId ?? commonId;
                    // TODO: check for business rec cache

                    context.Correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
                }
                else
                {
                    throw new BadRequestException("Failed invokation - unknown target.");
                }

                // check rules that need to be evaluated
                var matchingRule = await CheckArgumentRulesForPromotion(recommender, context);
                if (matchingRule != null)
                {
                    context.LogMessage($"Found argument rule ({matchingRule.Id}) matching argument {matchingRule.Argument?.CommonId}.");
                    // construct an output that works.
                    var outputByRule = new ItemsRecommenderModelOutputV1
                    {
                        ScoredItems = new List<ScoredRecommendableItem> { new ScoredRecommendableItem(matchingRule.Promotion, 1) }
                    };

                    await LoadReferences(outputByRule);
                    return await HandleRecommendation(recommender, context, input, outputByRule);
                }

                var segmentRule = await CheckArgumentRulesForSegment(recommender, context);

                // load the metrics for the invokation
                input.Metrics = await base.GetMetrics(recommender, context);

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
                    var allItems = await itemStore.Query(new EntityStoreQueryOptions<RecommendableItem>
                    {
                        PageSize = 12 // this is a fudge, it will only be the top 12 items
                    });
                    input.Items = allItems.Items;
                    logger.LogInformation($"Using all items as input, {input.Items.Count()} items");
                    invokationEntry.LogMessage($"Using all items as input, {input.Items.Count()} items");
                }

                IRecommenderModelClient<ItemsRecommenderModelOutputV1> client;
                if (recommender.UseOptimiser)
                {
                    client = await optimiserClientFactory.GetInternalOptimiserClient();
                }
                else if (recommender.ModelRegistration == null)
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
                if (recommender.OldArguments != null)
                {
                    foreach (var r in recommender.OldArguments)
                    {
                        CheckArgument(recommender, r, input, context);
                    }
                }

                var output = await client.Invoke(recommender, context, input);

                await LoadReferences(output);

                return await HandleRecommendation(recommender, context, input, output);
            }
            catch (CommonIdException commonIdEx)
            {
                await base.EndTrackInvokation(
                    context,
                    false,
                    message: $"Invoke failed for {commonIdEx.CommonId}",
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
                    logger.LogError($"Error invoking recommender model: ${ex.Message}");
                }

                if (recommender.ShouldThrowOnBadInput())
                {
                    await base.EndTrackInvokation(
                        context,
                        false,
                        message: "Invoke failed",
                        modelResponse: modelResponseContent,
                        saveOnComplete: true);
                    throw; // rethrow the error to propagate to calling client
                }
                else if (recommender.BaselineItemId != null)
                {
                    // case: baseline item and the model returned error
                    await itemsRecommenderStore.Load(recommender, _ => _.BaselineItem);
                    invokationEntry.LogMessage($"Model Error. Fallback to baseline item");
                    var output = new ItemsRecommenderModelOutputV1
                    {
                        ScoredItems = new List<ScoredRecommendableItem> { new ScoredRecommendableItem(recommender.BaselineItem!, 0) }
                    };
                    return await HandleRecommendation(recommender, context, input, output);
                }
                else
                {
                    // case: no default and the model returned error
                    var someItems = await itemStore.Query(new EntityStoreQueryOptions<RecommendableItem>());
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

        /// <summary>
        /// Loads the items from the database if required, and any discount codes.
        /// </summary>
        /// <param name="output">The model output.</param>
        /// <exception cref="ModelInvokationException">Throws if an item is invalid.</exception>
        private async Task LoadReferences(ItemsRecommenderModelOutputV1 output)
        {
            // load the items
            foreach (var scoredItem in output.ScoredItems)
            {
                if (scoredItem == null)
                {
                    throw new ModelInvokationException("Model returned a null ScoredItem");
                }
                else if (!string.IsNullOrEmpty(scoredItem.CommonId))
                {
                    scoredItem.Item ??= await itemStore.ReadFromCommonId(scoredItem.CommonId);
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
                    throw new ModelInvokationException("The model did not return a valid item." +
                     $"ItemCommonId: {scoredItem.ItemCommonId}, CommonId: {scoredItem.CommonId}, ItemId: {scoredItem.ItemId}, Score: {scoredItem.Score}");
                }

                scoredItem.DiscountCodes = await discountCodeWorkflow.GenerateDiscountCodes(scoredItem.Item);
            }
        }

        private async Task<ItemsRecommendation> HandleRecommendation(ItemsRecommender recommender,
                                                                     RecommendingContext context,
                                                                     IModelInput input,
                                                                     ItemsRecommenderModelOutputV1 output)
        {
            // produce the recommendation entity
            var recommendation = new ItemsRecommendation(recommender, context, output.ScoredItems);
            output.CorrelatorId = context.Correlator?.Id;
            recommendation.SetInput(input);
            recommendation.SetOutput(output);
            recommendation = await itemsRecommendationStore.Create(recommendation);
            context.LogMessage("Created a recommendation entity");

            var offer = new Offer(recommendation);
            offer = await offerStore.Create(offer);
            if (recommender.Settings?.RequireConsumptionEvent != true)
            {
                offer.State = OfferState.Presented;
            }
            recommendation.Offer = offer;
            context.LogMessage("Created an offer entity");

            // TODO: delete ?? - channel already replaced destinations
            // send to any destinations
            await base.SendToDestinations(recommender, context, recommendation);

            // send to channels
            await base.SendToChannels(recommender, context, recommendation);

            await base.EndTrackInvokation(context,
                                          true,
                                          message: $"Invoked successfully for {context.Customer?.Name ?? context.Customer?.CommonId}",
                                          modelResponse: null,
                                          saveOnComplete: true);

            // set this after the context has been saved.
            output.CorrelatorId = context.Correlator?.Id;
            return recommendation;
        }
    }
}
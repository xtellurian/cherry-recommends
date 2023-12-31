using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class PromotionsCampaignWorkflows : CampaignWorkflowBase<PromotionsCampaign>
    {
        private readonly IItemsRecommendationStore recommendationStore;
        private readonly IMetricStore metricStore;
        private readonly ICategoricalOptimiserClient optimiserClient;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IAudienceStore audienceStore;
        private readonly IRecommendableItemStore itemStore;
        private readonly IChannelStore channelStore;
        private readonly IPromotionOptimiserCRUDWorkflow promotionOptimiserCRUDWorkflow;

        public PromotionsCampaignWorkflows(
            IPromotionsCampaignStore store,
            IItemsRecommendationStore recommendationStore,
            IMetricStore metricStore,
            ICategoricalOptimiserClient optimiserClient,
            IModelRegistrationStore modelRegistrationStore,
            IAudienceStore audienceStore,
            IRecommenderReportImageWorkflow reportImageWorkflows,
            IRecommendableItemStore itemStore,
            IChannelStore channelStore,
            IStoreCollection storeCollection,
            IPromotionOptimiserCRUDWorkflow promotionOptimiserCRUDWorkflow) : base(store, storeCollection, reportImageWorkflows)
        {
            this.recommendationStore = recommendationStore;
            this.metricStore = metricStore;
            this.optimiserClient = optimiserClient;
            this.modelRegistrationStore = modelRegistrationStore;
            this.audienceStore = audienceStore;
            this.itemStore = itemStore;
            this.channelStore = channelStore;
            this.promotionOptimiserCRUDWorkflow = promotionOptimiserCRUDWorkflow;
        }

        public async Task<PromotionsCampaign> ClonePromotionsCampaign(CreateCommonEntityModel common, PromotionsCampaign from)
        {
            await store.Load(from, _ => _.BaselineItem);
            await store.LoadMany(from, _ => _.Items);
            await store.LoadMany(from, _ => _.Channels);

            var fromAudience = await audienceStore.GetAudience(from);
            var segmentIds = fromAudience.Success ? fromAudience.Entity!.Segments.Select(_ => _.Id) : Enumerable.Empty<long>();
            var channelIds = from.Channels.Count > 0 ? from.Channels.Select(_ => _.Id) : Enumerable.Empty<long>();

            return await CreatePromotionsCampaign(common,
                                                  from.BaselineItem?.CommonId,
                                                  from.Items?.Select(_ => _.CommonId),
                                                  segmentIds,
                                                  channelIds,
                                                  from.NumberOfItemsToRecommend,
                                                  from.Arguments,
                                                  from.ErrorHandling ?? new CampaignSettings(),
                                                  from.UseOptimiser,
                                                  from.TargetMetric?.CommonId,
                                                  from.TargetType,
                                                  useInternalId: false);
        }

        public async Task<CampaignStatistics> CalculateStatistics(PromotionsCampaign recommender)
        {
            var stats = new CampaignStatistics();
            stats.NumberCustomersRecommended = await recommendationStore.CountUniqueCustomers(recommender.Id);
            stats.NumberInvokations = await recommendationStore.CountRecommendations(recommender.Id);
            return stats;
        }

        public async Task<PromotionsCampaign> CreatePromotionsCampaign(CreateCommonEntityModel common,
                                                                   string? baselineItemId,
                                                                   IEnumerable<string>? itemIds,
                                                                   IEnumerable<long>? segmentIds,
                                                                   IEnumerable<long>? channelIds,
                                                                   int? numberOfItemsToRecommend,
                                                                   IEnumerable<CampaignArgument>? arguments,
                                                                   CampaignSettings settings,
                                                                   bool useOptimiser,
                                                                   string? targetMetricId,
                                                                   PromotionCampaignTargetTypes targetType,
                                                                   bool? useInternalId)
        {
            RecommendableItem baselineItem;
            if (!string.IsNullOrEmpty(baselineItemId))
            {
                baselineItem = await itemStore.GetEntity(baselineItemId, useInternalId: useInternalId);
            }
            else
            {
                throw new BadRequestException("BaselineItem is required");
            }

            Metric? targetMetric = null;
            if (!string.IsNullOrEmpty(targetMetricId))
            {
                targetMetric = await metricStore.GetEntity(targetMetricId);
            }

            // link the required promotions aka items.
            var promotions = new List<RecommendableItem>();
            if (itemIds.IsNullOrEmpty())
            {
                throw new BadRequestException("ItemIds must contain a promotion ID");
            }
            else
            {
                foreach (var id in itemIds!)
                {
                    promotions.Add(await itemStore.GetEntity(id, useInternalId));
                }
            }
            // add the default item if it wasn't added by the user.
            if (!promotions.Any(_ => _.Id == baselineItem.Id))
            {
                promotions.Add(baselineItem);
            }


            PromotionsCampaign recommender = await store.Create(
                new PromotionsCampaign(common.CommonId, common.Name, baselineItem, promotions, arguments, settings, targetMetric)
                {
                    NumberOfItemsToRecommend = numberOfItemsToRecommend ?? 1,
                    TargetType = targetType
                });

            if (segmentIds != null && segmentIds.Any())
            {
                var segments = new List<Segment>();
                foreach (var segmentId in segmentIds)
                {
                    segments.Add(await segmentStore.Read(segmentId));
                }
                if (segments.Any())
                {
                    var audience = new Audience(recommender, segments);
                    await audienceStore.Create(audience);
                }
            }

            if (channelIds != null && channelIds.Count() > recommender.MaxChannelCount)
            {
                throw new BadRequestException($"The maximum number of channels is {recommender.MaxChannelCount}");
            }

            if (channelIds != null && channelIds.Any())
            {
                foreach (var channelId in channelIds)
                {
                    recommender.Channels.Add(await channelStore.Read(channelId));
                }
            }

            if (useOptimiser)
            {
                await promotionOptimiserCRUDWorkflow.Create(recommender);
                recommender.UseOptimiser = true;
            }

            await store.Context.SaveChanges();
            return recommender;
        }

        public async Task<RecommendableItem> SetBaselineItem(PromotionsCampaign recommender, string itemId)
        {
            var item = await itemStore.GetEntity(itemId);
            recommender.BaselineItem = item;
            await store.Update(recommender);
            await store.Context.SaveChanges();
            return item;
        }

        public async Task<RecommendableItem> AddItem(PromotionsCampaign recommender, RecommendableItem item, bool? useInternalId = null)
        {
            await store.LoadMany(recommender, _ => _.Items);

            if (!recommender.Items.Any(_ => _.Id == item.Id)) // if the item not already in there.
            {
                recommender.Items.Add(item);
                await store.Context.SaveChanges();
            }

            return item;
        }
        public async Task<RecommendableItem> AddItem(PromotionsCampaign recommender, string itemId, bool? useInternalId = null)
        {
            var item = await itemStore.GetEntity(itemId, useInternalId);
            return await AddItem(recommender, item, useInternalId);
        }
        public async Task<RecommendableItem> AddItem(PromotionsCampaign recommender, long itemId, bool? useInternalId = null)
        {
            var item = await itemStore.Read(itemId);
            return await AddItem(recommender, item, useInternalId);
        }

        public async Task<RecommendableItem> RemoveItem(PromotionsCampaign recommender, string itemId, bool? useInternalId = null)
        {
            var item = await itemStore.GetEntity(itemId, useInternalId);
            await store.LoadMany(recommender, _ => _.Items);

            if (recommender.Items.Any(_ => _.Id == item.Id)) // if the item is in there.
            {
                item = recommender.Items.First(_ => _.Id == item.Id); // ensure the item is the right object
                recommender.Items.Remove(item);
                await store.Context.SaveChanges();
            }

            return item;
        }

        public async Task<Paginated<RecommendableItem>> QueryItems(IPaginate paginate, string recommenderId, bool? useInternalId)
        {
            var recommender = await store.GetEntity(recommenderId, useInternalId);
            return await itemStore.QueryForRecommender(paginate, recommender.Id);
        }

        public async Task<Paginated<ItemsRecommendation>> QueryRecommendations(string recommenderId, IPaginate paginate, bool? useInternalId = null)
        {
            var recommender = await store.GetEntity(recommenderId, useInternalId);
            return await recommendationStore.QueryForRecommender(paginate, recommender.Id);
        }

        public async Task<ModelRegistration> LinkRegisteredModel(PromotionsCampaign recommender, long modelId)
        {
            var model = await modelRegistrationStore.Read(modelId);
            if (model.ModelType == ModelTypes.ItemsRecommenderV1)
            {
                recommender.ModelRegistration = model;
                await store.Context.SaveChanges();
                return model;
            }
            else
            {
                throw new BadRequestException($"Model of type {model.ModelType} can't be linked to an Items Recommender");
            }
        }
    }
}
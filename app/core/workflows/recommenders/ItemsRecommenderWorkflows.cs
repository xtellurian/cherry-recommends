using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ItemsRecommenderWorkflows : RecommenderWorkflowBase<ItemsRecommender>
    {
        private readonly IStorageContext storageContext;
        private readonly IItemsRecommendationStore recommendationStore;
        private readonly IMetricStore metricStore;
        private readonly ICategoricalOptimiserClient optimiserClient;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IRecommendableItemStore itemStore;

        public ItemsRecommenderWorkflows(
            IStorageContext storageContext,
            IItemsRecommenderStore store,
            IItemsRecommendationStore recommendationStore,
            IMetricStore metricStore,
            ISegmentStore segmentStore,
            IIntegratedSystemStore systemStore,
            ICategoricalOptimiserClient optimiserClient,
            IModelRegistrationStore modelRegistrationStore,
            RecommenderReportImageWorkflows reportImageWorkflows,
            IRecommendableItemStore itemStore) : base(store, systemStore, metricStore, segmentStore, reportImageWorkflows)
        {
            this.storageContext = storageContext;
            this.recommendationStore = recommendationStore;
            this.metricStore = metricStore;
            this.optimiserClient = optimiserClient;
            this.modelRegistrationStore = modelRegistrationStore;
            this.itemStore = itemStore;
        }

        public async Task<ItemsRecommender> CloneItemsRecommender(CreateCommonEntityModel common, ItemsRecommender from)
        {
            await store.Load(from, _ => _.BaselineItem);
            await store.LoadMany(from, _ => _.Items);
            return await CreateItemsRecommender(common,
                                                  from.BaselineItem?.CommonId,
                                                  from.Items?.Select(_ => _.CommonId),
                                                  from.NumberOfItemsToRecommend,
                                                  from.Arguments,
                                                  from.ErrorHandling ?? new RecommenderSettings(),
                                                  true,
                                                  from.TargetMetric?.CommonId,
                                                  from.TargetType,
                                                  useInternalId: true);
        }

        public async Task<RecommenderStatistics> CalculateStatistics(ItemsRecommender recommender)
        {
            var stats = new RecommenderStatistics();
            stats.NumberCustomersRecommended = await recommendationStore.CountUniqueCustomers(recommender.Id);
            stats.NumberInvokations = await recommendationStore.CountRecommendations(recommender.Id);
            return stats;
        }

        public async Task<ItemsRecommender> CreateItemsRecommender(CreateCommonEntityModel common,
                                                                   string? baselineItemId,
                                                                   IEnumerable<string>? itemIds,
                                                                   int? numberOfItemsToRecommend,
                                                                   IEnumerable<RecommenderArgument>? arguments,
                                                                   RecommenderSettings settings,
                                                                   bool useOptimiser,
                                                                   string? targetMetricId,
                                                                   PromotionRecommenderTargetTypes targetType,
                                                                   bool? useInternalId)
        {
            RecommendableItem? baselineItem = null;
            if (!string.IsNullOrEmpty(baselineItemId))
            {
                baselineItem = await itemStore.GetEntity(baselineItemId, useInternalId: useInternalId);
            }

            Metric? targetMetric = null;
            if (!string.IsNullOrEmpty(targetMetricId))
            {
                targetMetric = await metricStore.GetEntity(targetMetricId);
            }

            ItemsRecommender recommender;
            if (itemIds != null && itemIds.Any())
            {
                var items = new List<RecommendableItem>();
                foreach (var id in itemIds)
                {
                    items.Add(await itemStore.GetEntity(id, useInternalId));
                }

                recommender = await store.Create(
                    new ItemsRecommender(common.CommonId, common.Name, baselineItem, items, arguments, settings, targetMetric)
                    {
                        NumberOfItemsToRecommend = numberOfItemsToRecommend,
                        TargetType = targetType
                    });

            }
            else
            {
                recommender = await store.Create(
                    new ItemsRecommender(common.CommonId, common.Name, baselineItem, null, arguments, settings, targetMetric)
                    {
                        NumberOfItemsToRecommend = numberOfItemsToRecommend,
                        TargetType = targetType
                    });
            }

            if (useOptimiser)
            {
                var registration = new ModelRegistration(
                    System.Guid.NewGuid().ToString(), ModelTypes.ItemsRecommenderV1, HostingTypes.AzureFunctions, null, null, null);
                recommender.ModelRegistration = registration;
                var optimiser = await optimiserClient.Create(recommender);
            }
            await storageContext.SaveChanges();
            return recommender;
        }

        public async Task<RecommendableItem> SetBaselineItem(ItemsRecommender recommender, string itemId)
        {
            var item = await itemStore.GetEntity(itemId);
            recommender.BaselineItem = item;
            await store.Update(recommender);
            await storageContext.SaveChanges();
            return item;
        }

        public async Task<RecommendableItem> AddItem(ItemsRecommender recommender, RecommendableItem item, bool? useInternalId = null)
        {
            await store.LoadMany(recommender, _ => _.Items);

            if (!recommender.Items.Any(_ => _.Id == item.Id)) // if the item not already in there.
            {
                recommender.Items.Add(item);
                await store.Context.SaveChanges();
            }

            return item;
        }
        public async Task<RecommendableItem> AddItem(ItemsRecommender recommender, string itemId, bool? useInternalId = null)
        {
            var item = await itemStore.GetEntity(itemId, useInternalId);
            return await AddItem(recommender, item, useInternalId);
        }
        public async Task<RecommendableItem> AddItem(ItemsRecommender recommender, long itemId, bool? useInternalId = null)
        {
            var item = await itemStore.Read(itemId);
            return await AddItem(recommender, item, useInternalId);
        }

        public async Task<RecommendableItem> RemoveItem(ItemsRecommender recommender, string itemId, bool? useInternalId = null)
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

        public async Task<Paginated<RecommendableItem>> QueryItems(string recommenderId, int page, bool? useInternalId)
        {
            var recommender = await store.GetEntity(recommenderId, useInternalId);
            return await itemStore.QueryForRecommender(recommender.Id, page);
        }

        public async Task<Paginated<ItemsRecommendation>> QueryRecommendations(string recommenderId, int page, int? pageSize, bool? useInternalId = null)
        {
            var recommender = await store.GetEntity(recommenderId, useInternalId);
            return await recommendationStore.QueryForRecommender(page, pageSize, recommender.Id);
        }

        public async Task<ModelRegistration> LinkRegisteredModel(ItemsRecommender recommender, long modelId)
        {
            var model = await modelRegistrationStore.Read(modelId);
            if (model.ModelType == ModelTypes.ItemsRecommenderV1)
            {
                recommender.ModelRegistration = model;
                await storageContext.SaveChanges();
                return model;
            }
            else
            {
                throw new BadRequestException($"Model of type {model.ModelType} can't be linked to an Items Recommender");
            }
        }
    }
}
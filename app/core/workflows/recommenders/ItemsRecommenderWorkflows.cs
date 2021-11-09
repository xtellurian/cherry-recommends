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
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IRecommendableItemStore itemStore;

        public ItemsRecommenderWorkflows(
            IStorageContext storageContext,
            IItemsRecommenderStore store,
            IItemsRecommendationStore recommendationStore,
            IFeatureStore featureStore,
            IIntegratedSystemStore systemStore,
            IModelRegistrationStore modelRegistrationStore,
            IRecommendableItemStore itemStore) : base(store, systemStore, featureStore)
        {
            this.storageContext = storageContext;
            this.recommendationStore = recommendationStore;
            this.modelRegistrationStore = modelRegistrationStore;
            this.itemStore = itemStore;
        }

        public async Task<ItemsRecommender> CloneItemsRecommender(CreateCommonEntityModel common, ItemsRecommender from)
        {
            await store.Load(from, _ => _.DefaultItem);
            await store.LoadMany(from, _ => _.Items);
            return await CreateItemsRecommender(common,
                                                  from.DefaultItem?.CommonId,
                                                  from.Items?.Select(_ => _.CommonId),
                                                  from.NumberOfItemsToRecommend,
                                                  from.Arguments,
                                                  from.ErrorHandling ?? new RecommenderSettings());
        }
        public async Task<ItemsRecommender> CreateItemsRecommender(CreateCommonEntityModel common,
                                                                       string? defaultItemId,
                                                                       IEnumerable<string>? itemsCommonIds,
                                                                       int? numberOfItemsToRecommend,
                                                                       IEnumerable<RecommenderArgument>? arguments,
                                                                       RecommenderSettings settings)
        {
            RecommendableItem? defaultItem = null;
            if (!string.IsNullOrEmpty(defaultItemId))
            {
                defaultItem = await itemStore.GetEntity(defaultItemId);
            }

            if (itemsCommonIds != null && itemsCommonIds.Any())
            {
                var items = new List<RecommendableItem>();
                foreach (var id in itemsCommonIds)
                {
                    items.Add(await itemStore.ReadFromCommonId(id));
                }

                var recommender = await store.Create(
                    new ItemsRecommender(common.CommonId, common.Name, defaultItem, items, arguments, settings)
                    { NumberOfItemsToRecommend = numberOfItemsToRecommend });
                await storageContext.SaveChanges();
                return recommender;
            }
            else
            {
                var recommender = await store.Create(
                    new ItemsRecommender(common.CommonId, common.Name, defaultItem, null, arguments, settings)
                    { NumberOfItemsToRecommend = numberOfItemsToRecommend });
                await storageContext.SaveChanges();
                return recommender;
            }
        }

        public async Task<RecommendableItem> SetDefaultItem(ItemsRecommender recommender, string itemId)
        {
            var item = await itemStore.GetEntity(itemId);
            recommender.DefaultItem = item;
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

        public async Task<Paginated<ItemsRecommendation>> QueryRecommendations(string recommenderId, int page, bool? useInternalId = null)
        {
            var recommender = await store.GetEntity(recommenderId, useInternalId);
            return await recommendationStore.QueryForRecommender(page, recommender.Id);
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
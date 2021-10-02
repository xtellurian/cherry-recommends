using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ItemsRecommenderWorkflows
    {
        private readonly IStorageContext storageContext;
        private readonly IItemsRecommenderStore store;
        private readonly IItemsRecommendationStore recommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IRecommendableItemStore itemStore;

        public ItemsRecommenderWorkflows(IStorageContext storageContext,
                                                IItemsRecommenderStore store,
                                                IItemsRecommendationStore recommendationStore,
                                                IModelRegistrationStore modelRegistrationStore,
                                                IRecommendableItemStore itemStore)
        {
            this.storageContext = storageContext;
            this.store = store;
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

        public async Task<Paginated<ItemsRecommendation>> QueryRecommendations(long recommenderId, int page)
        {
            return await recommendationStore.QueryForRecommender(page, recommenderId);
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
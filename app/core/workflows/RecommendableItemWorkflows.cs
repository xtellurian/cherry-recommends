using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class RecommendableItemWorkflows : IWorkflow
    {
        private readonly IRecommendableItemStore store;
        private readonly IStorageContext storageContext;

        public RecommendableItemWorkflows(IRecommendableItemStore store, IStorageContext storageContext)
        {
            this.store = store;
            this.storageContext = storageContext;
        }

        public async Task<RecommendableItem> CreateRecommendableItem(string commonId,
                                                                     string name,
                                                                     double? listPrice,
                                                                     double? directCost,
                                                                     string? description,
                                                                     DynamicPropertyDictionary? properties)
        {
            properties?.Validate();
            var item = await store.Create(new RecommendableItem(commonId, name, listPrice ?? 1, directCost, properties)
            {
                Description = description,
            });

            await storageContext.SaveChanges();
            return item;
        }
        public async Task<RecommendableItem> UpdateRecommendableItem(RecommendableItem item,
                                                                     string name,
                                                                     double? listPrice,
                                                                     double? directCost,
                                                                     string? description,
                                                                     DynamicPropertyDictionary? properties)
        {
            properties?.Validate();
            item.Name = name;
            item.ListPrice = listPrice;
            item.DirectCost = directCost;
            item.Description = description;
            item.Properties = properties;

            await store.Update(item);
            await storageContext.SaveChanges();
            return item;
        }
    }
}
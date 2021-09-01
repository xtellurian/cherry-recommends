using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
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
                                                                     string description = null,
                                                                     double? directCost = null)
        {
            var item = await store.Create(new RecommendableItem(commonId, name, listPrice ?? 1, directCost)
            {
                Description = description,
            });

            await storageContext.SaveChanges();
            return item;
        }
    }
}
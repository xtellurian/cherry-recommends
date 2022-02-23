using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class RecommendableItemWorkflows : IWorkflow
    {
        private readonly IRecommendableItemStore store;
        private readonly IItemsRecommenderStore recommenderStore;
        private readonly IStorageContext storageContext;

        public RecommendableItemWorkflows(IRecommendableItemStore store, IItemsRecommenderStore recommenderStore, IStorageContext storageContext)
        {
            this.store = store;
            this.recommenderStore = recommenderStore;
            this.storageContext = storageContext;
        }

        public async Task<RecommendableItem> CreateRecommendableItem(string commonId,
                                                                     string name,
                                                                     double? directCost,
                                                                     int numberOfRedemptions,
                                                                     BenefitType benefitType,
                                                                     double benefitValue,
                                                                     PromotionType promotionType,
                                                                     string? description,
                                                                     DynamicPropertyDictionary? properties)
        {
            properties?.Validate();
            var item = await store.Create(new RecommendableItem(commonId, name, directCost, numberOfRedemptions, benefitType, benefitValue, promotionType, properties)
            {
                Description = description,
            });

            await storageContext.SaveChanges();
            return item;
        }

        public async Task<bool> IsBaselineItemForRecommender(RecommendableItem item)
        {
            var result = await recommenderStore.Count(_ => _.BaselineItemId == item.Id);
            return result > 0;
        }

        public async Task<RecommendableItem> UpdateRecommendableItem(RecommendableItem item,
                                                                     string name,
                                                                     double? directCost,
                                                                     int numberOfRedemptions,
                                                                     BenefitType benefitType,
                                                                     double benefitValue,
                                                                     PromotionType promotionType,
                                                                     string? description,
                                                                     DynamicPropertyDictionary? properties)
        {
            properties?.Validate();
            item.Name = name;
            item.DirectCost = directCost;
            item.NumberOfRedemptions = numberOfRedemptions;
            item.BenefitType = benefitType;
            item.BenefitValue = benefitValue;
            item.PromotionType = promotionType;
            item.Description = description;
            item.Properties = properties;

            await store.Update(item);
            await storageContext.SaveChanges();
            return item;
        }
    }
}
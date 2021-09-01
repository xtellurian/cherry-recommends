using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRecommendableItemStore : EFCommonEntityStoreBase<RecommendableItem>, IRecommendableItemStore
    {
        public EFRecommendableItemStore(SignalBoxDbContext context)
        : base(context, (c) => c.RecommendableItems)
        {
        }
    }
}
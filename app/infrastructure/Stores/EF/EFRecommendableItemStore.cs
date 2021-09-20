using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRecommendableItemStore : EFCommonEntityStoreBase<RecommendableItem>, IRecommendableItemStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFRecommendableItemStore(SignalBoxDbContext context, IEnvironmentService environmentService)
        : base(context, environmentService, (c) => c.RecommendableItems)
        {
        }
    }
}
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRecommendableItemStore : EFCommonEntityStoreBase<RecommendableItem>, IRecommendableItemStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFRecommendableItemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, (c) => c.RecommendableItems)
        { }
    }
}
using SignalBox.Core;
using SignalBox.Core.Optimisers;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFPromotionOptimiserStore : EFEnvironmentScopedEntityStoreBase<PromotionOptimiser>, IPromotionOptimiserStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFPromotionOptimiserStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, c => c.PromotionOptimisers)
        { }

    }
}
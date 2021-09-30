using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRecommendationCorrelatorStore : EFEntityStoreBase<RecommendationCorrelator>, IRecommendationCorrelatorStore
    {
        public EFRecommendationCorrelatorStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.RecommendationCorrelators)
        { }
    }
}
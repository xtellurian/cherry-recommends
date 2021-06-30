using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRecommendationCorrelatorStore : EFEntityStoreBase<RecommendationCorrelator>, IRecommendationCorrelatorStore
    {
        public EFRecommendationCorrelatorStore(SignalBoxDbContext context)
        : base(context, (c) => c.RecommendationCorrelators)
        {
        }
    }
}
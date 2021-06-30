using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFOfferRecommendationStore : EFEntityStoreBase<OfferRecommendation>, IOfferRecommendationStore
    {
        public EFOfferRecommendationStore(SignalBoxDbContext context)
        : base(context, (c) => c.Recommendations)
        {
        }
    }
}
using SignalBox.Core;

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
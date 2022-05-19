using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IOfferStore : IEntityStore<Offer>
    {
        Task<EntityResult<Offer>> ReadOfferByRecommendation(ItemsRecommendation recommendation);
        Task<EntityResult<Offer>> ReadOfferByRecommendationCorrelator(long recommendationCorrelatorId);
        Task<IEnumerable<Offer>> ReadOffersForCustomer(Customer customer);
    }
}
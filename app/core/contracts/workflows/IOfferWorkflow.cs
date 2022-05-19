using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IOfferWorkflow
    {
        Task RedeemOffer(CustomerEvent customerEvent);
        Task<Paginated<Offer>> QueryOffers(ItemsRecommender recommender, IPaginate paginate, OfferState? state = null);
    }
}
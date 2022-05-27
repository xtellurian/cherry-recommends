using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IOfferWorkflow
    {
        /// <summary>
        /// Updates an existing offer based on a customer event.
        /// </summary>
        /// <param name="customerEvent"></param>
        /// <returns></returns>
        Task UpdateOffer(CustomerEvent customerEvent);
        Task<Paginated<Offer>> QueryOffers(PromotionsCampaign campaign, IPaginate paginate, OfferState? state = null);
    }
}
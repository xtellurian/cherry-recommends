using System.Threading.Tasks;
using SignalBox.Core.Campaigns;
using System.Collections.Generic;

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
        Task<IEnumerable<ARPOReportData>> QueryARPOReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11);
        Task<IEnumerable<OfferConversionRateData>> QueryConversionRateReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11);
    }
}
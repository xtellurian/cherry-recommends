using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Campaigns;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IOfferStore : IEntityStore<Offer>
    {
        Task<EntityResult<Offer>> ReadOfferByRecommendation(ItemsRecommendation recommendation);
        Task<EntityResult<Offer>> ReadOfferByRecommendationCorrelator(long recommendationCorrelatorId);
        Task<IEnumerable<Offer>> ReadOffersForCustomer(Customer customer);
        /// <summary>
        /// Queries the Average Revenue per Offer report data.
        /// <param name="campaign"></param>
        /// <param name="period"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        /// </summary>
        Task<IEnumerable<ARPOReportData>> QueryARPOReportData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate);
        /// <summary>
        /// Queries the Offer Conversion Rate report data.
        /// <param name="campaign"></param>
        /// <param name="period"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        /// </summary>
        Task<IEnumerable<OfferConversionRateData>> QueryConversionRateData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate);
    }
}
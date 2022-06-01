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
        /// <param name="type"></param>
        /// <param name="startDate"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// </summary>
        Task<IEnumerable<OfferMeanGrossRevenue>> QueryARPOReportData(PromotionsCampaign campaign, ARPOReportType type, DateTimeOffset startDate, OfferState state);
    }
}
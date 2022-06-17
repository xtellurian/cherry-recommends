using System.Threading.Tasks;
using SignalBox.Core;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core.Recommendations;
using System.Collections.Generic;
using System.Linq;
using System;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFOfferStore : EFEnvironmentScopedEntityStoreBase<Offer>, IOfferStore
    {
        protected override bool IsEnvironmentScoped => true;

        public EFOfferStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.Offers)
        { }

        public async Task<EntityResult<Offer>> ReadOfferByRecommendation(ItemsRecommendation recommendation)
        {
            var offer = await QuerySet.FirstOrDefaultAsync(_ => _.RecommendationId == recommendation.Id);
            return new EntityResult<Offer>(offer);
        }

        public async Task<EntityResult<Offer>> ReadOfferByRecommendationCorrelator(long recommendationCorrelatorId)
        {
            var offer = await QuerySet.FirstOrDefaultAsync(_ => _.Recommendation.RecommendationCorrelatorId == recommendationCorrelatorId);
            return new EntityResult<Offer>(offer);
        }

        public async Task<IEnumerable<Offer>> ReadOffersForCustomer(Customer customer, params OfferState[] offerStates)
        {
            if (offerStates.IsNullOrEmpty())
            {
                return await QuerySet
                    .Where(_ => _.Recommendation.CustomerId == customer.Id)
                    .Include(_ => _.RedeemedPromotion)
                    .Include(_ => _.Recommendation)
                    .ThenInclude(_ => _.Items)
                    .ToListAsync();
            }
            else
            {
                return await QuerySet
                    .Where(_ => _.Recommendation.CustomerId == customer.Id && offerStates.Contains(_.State))
                    .Include(_ => _.RedeemedPromotion)
                    .Include(_ => _.Recommendation)
                    .ThenInclude(_ => _.Items)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryARPOReportData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate)
        {
            var results = await context.OfferMeanGrossRevenues
                .FromSqlInterpolated($"dbo.sp_ARPO {campaign.Id}, {period}, {startDate}, {environmentProvider.CurrentEnvironmentId}")
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryAPVReportData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate)
        {
            var results = await context.OfferMeanGrossRevenues
                .FromSqlInterpolated($"dbo.sp_APV {campaign.Id}, {period}, {startDate}, {environmentProvider.CurrentEnvironmentId}")
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<OfferConversionRateData>> QueryConversionRateData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate)
        {
            var results = await context.OfferConversionRates
                .FromSqlInterpolated($"dbo.sp_OfferConversionRate {campaign.Id}, {period}, {startDate}, {environmentProvider.CurrentEnvironmentId}")
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<OfferSensitivityCurveData>> QueryOfferSensitivityCurveData(PromotionsCampaign campaign, DateTimeOffset startDate)
        {
            var results = await context.OfferSensitivityCurveData
                .FromSqlInterpolated($"dbo.sp_OfferSensitivityCurve {campaign.Id}, {startDate}")
                .ToListAsync();
            return results;
        }
    }
}
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

        public async Task<IEnumerable<Offer>> ReadOffersForCustomer(Customer customer)
        {
            var results = await QuerySet.Where(_ => _.Recommendation.CustomerId == customer.Id).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryARPOReportData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate, OfferState state)
        {
            var results = await context.OfferMeanGrossRevenues
                .FromSqlInterpolated($"dbo.sp_OfferMeanGrossRevenue {campaign.Id}, {period}, {startDate}, {state.ToString()}")
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<OfferConversionRateData>> QueryConversionRateData(PromotionsCampaign campaign, DateTimePeriod period, DateTimeOffset startDate, long? environmentId)
        {
            var results = await context.OfferConversionRates
                .FromSqlInterpolated($"dbo.sp_OfferConversionRate {campaign.Id}, {period}, {startDate}, {environmentId}")
                .ToListAsync();
            return results;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
    public class OfferWorkflows : IOfferWorkflow, IWorkflow
    {
        private readonly ILogger<OfferWorkflows> logger;
        private readonly IOfferStore offerStore;
        private readonly IRecommendableItemStore promotionStore;
        private readonly IItemsRecommendationStore promotionsRecommendationStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEnvironmentProvider environmentProvider;

        public OfferWorkflows(
            ILogger<OfferWorkflows> logger,
            IOfferStore offerStore,
            IRecommendableItemStore promotionStore,
            IItemsRecommendationStore promotionsRecommendationStore,
            IDateTimeProvider dateTimeProvider,
            IEnvironmentProvider environmentProvider)
        {
            this.logger = logger;
            this.offerStore = offerStore;
            this.promotionStore = promotionStore;
            this.promotionsRecommendationStore = promotionsRecommendationStore;
            this.dateTimeProvider = dateTimeProvider;
            this.environmentProvider = environmentProvider;
        }

        public async Task UpdateOffer(CustomerEvent customerEvent)
        {
            var offerEventKinds = new EventKinds[]
            {
                EventKinds.Purchase,
                EventKinds.UsePromotion,
                EventKinds.PromotionPresented
            };
            bool isOfferEventKind = offerEventKinds.Any(_ => _ == customerEvent.EventKind);
            bool isRedeemEvent = customerEvent.EventKind == EventKinds.Purchase ||
                customerEvent.EventKind == EventKinds.UsePromotion;

            // Only process valid offer events
            if (!isOfferEventKind ||
                !customerEvent.RecommendationCorrelatorId.HasValue)
            {
                return;
            }

            var result = await offerStore.ReadOfferByRecommendationCorrelator(customerEvent.RecommendationCorrelatorId.Value);
            if (result.Success)
            {
                var now = dateTimeProvider.Now;
                var offer = result.Entity;

                if (customerEvent.EventKind == EventKinds.PromotionPresented && offer.State == OfferState.Created)
                {
                    offer.State = OfferState.Presented;
                    offer.LastUpdated = now;
                    logger.LogInformation("Offer {offerId} presented", offer.Id);
                }
                else if (isRedeemEvent && offer.State != OfferState.Redeemed)
                {
                    long? promotionId = TryGetValue<long>(customerEvent.Properties, "promotionId");
                    if (promotionId.HasValue)
                    {
                        var promotion = await promotionStore.Read(promotionId.Value);
                        offer.RedeemedPromotionId = promotion.Id;
                        offer.RedeemedPromotion = promotion;
                    }
                    offer.State = OfferState.Redeemed;
                    offer.LastUpdated = now;
                    offer.RedeemedAt = now;

                    if (customerEvent.EventKind == EventKinds.Purchase)
                    {
                        float? value = TryGetValue<float>(customerEvent.Properties, "value");
                        offer.GrossRevenue = value > 0 ? value : 1;
                    }
                    logger.LogInformation("Offer {offerId} redeemed using promotion {promotionId}", offer.Id, promotionId);
                }

                await offerStore.Update(offer);
                // Do not call Context.SaveChanges() for better performance. 
                // Calling method should handle this.
            }
        }

        private T? TryGetValue<T>(DynamicPropertyDictionary properties, string key)
                where T : struct
        {
            object value = null;
            if (properties.TryGetValue(key, out object _value))
            {
                if (_value != null)
                {
                    value = _value.ToString();
                }
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T?);
            }
        }

        public async Task<Paginated<Offer>> QueryOffers(PromotionsCampaign recommender, IPaginate paginate, OfferState? state = null)
        {
            List<Expression<Func<Offer, bool>>> expressions = new List<Expression<Func<Offer, bool>>>();
            Expression<Func<Offer, bool>> predicate = null;

            expressions.Add(_ => _.Recommendation.Recommender.Id == recommender.Id);
            if (state.HasValue)
            {
                expressions.Add(_ => _.State == state);
            }

            foreach (var expression in expressions)
            {
                predicate = predicate != null ? predicate.And(expression) : expression;
            }

            return await offerStore.Query(paginate, predicate);
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryDailyARPOReportData(PromotionsCampaign campaign, int daysAgo = 7)
        {
            var startDate = dateTimeProvider.Now.ToUniversalTime().AddDays(-1 * daysAgo).TruncateToDayStart();
            return await offerStore.QueryARPOReportData(campaign, DateTimePeriod.Daily, startDate, OfferState.Redeemed);
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryWeeklyARPOReportData(PromotionsCampaign campaign, int weeksAgo = 11)
        {
            var weeksAgoDt = dateTimeProvider.Now.ToUniversalTime().AddDays(-7 * weeksAgo);
            var startDate = weeksAgoDt.FirstDayOfWeek(DayOfWeek.Monday);
            return await offerStore.QueryARPOReportData(campaign, DateTimePeriod.Weekly, startDate, OfferState.Redeemed);
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryMonthlyARPOReportData(PromotionsCampaign campaign, int monthsAgo = 11)
        {
            var startDate = dateTimeProvider.Now.ToUniversalTime().AddMonths(-monthsAgo);
            return await offerStore.QueryARPOReportData(campaign, DateTimePeriod.Monthly, startDate, OfferState.Redeemed);
        }

        public async Task<IEnumerable<OfferConversionRateData>> QueryConversionRateReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11)
        {
            DateTimeOffset startDate = dateTimeProvider.Now.ToUniversalTime().DateTimeSince(period, periodAgo);
            return await offerStore.QueryConversionRateData(campaign, period, startDate, environmentProvider.CurrentEnvironmentId);
        }
    }
}
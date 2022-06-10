using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class OfferWorkflows : IOfferWorkflow, IWorkflow
    {
        private readonly ILogger<OfferWorkflows> logger;
        private readonly IOfferStore offerStore;
        private readonly IRecommendableItemStore promotionStore;
        private readonly IItemsRecommendationStore promotionsRecommendationStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public OfferWorkflows(
            ILogger<OfferWorkflows> logger,
            IOfferStore offerStore,
            IRecommendableItemStore promotionStore,
            IItemsRecommendationStore promotionsRecommendationStore,
            IDateTimeProvider dateTimeProvider)
        {
            this.logger = logger;
            this.offerStore = offerStore;
            this.promotionStore = promotionStore;
            this.promotionsRecommendationStore = promotionsRecommendationStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        private async Task<EntityResult<Offer>> TryLoadOffer(CustomerEvent customerEvent)
        {
            var offerEventKinds = new EventKinds[]
           {
                EventKinds.Purchase,
                EventKinds.UsePromotion,
                EventKinds.PromotionPresented
           };
            bool isOfferEventKind = offerEventKinds.Any(_ => _ == customerEvent.EventKind);
            // Only process valid offer events
            if (!isOfferEventKind)
            {
                return new EntityResult<Offer>(null);
            }
            if (customerEvent.RecommendationCorrelatorId.HasValue)
            {
                return await offerStore.ReadOfferByRecommendationCorrelator(customerEvent.RecommendationCorrelatorId.Value);
            }
            // now, try to load based properties
            var offers = await offerStore.ReadOffersForCustomer(customerEvent.Customer, OfferState.Created, OfferState.Presented);

            Offer? offer = null;
            if (customerEvent.TryGetPromotionId(out var promotionId))
            {
                if (long.TryParse(promotionId, out var internalPromoId))
                {
                    offer = offers.FirstOrDefault(_ => _.Recommendation.Items.Any(_ => _.Id == internalPromoId || _.CommonId == promotionId));
                }
                else
                {
                    offer = offers.FirstOrDefault(_ => _.Recommendation.Items.Any(_ => _.CommonId == promotionId));
                }
            }

            return new EntityResult<Offer>(offer);
        }

        public async Task UpdateOffer(CustomerEvent customerEvent)
        {
            bool isRedeemEvent = customerEvent.EventKind == EventKinds.Purchase ||
               customerEvent.EventKind == EventKinds.UsePromotion;
            var result = await TryLoadOffer(customerEvent);

            if (result.Success && result.Entity != null)
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
            object? value = null;
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
                return default;
            }
        }

        public async Task<Paginated<Offer>> QueryOffers(PromotionsCampaign recommender, IPaginate paginate, OfferState? state = null)
        {
            List<Expression<Func<Offer, bool>>> expressions = new List<Expression<Func<Offer, bool>>>();
            Expression<Func<Offer, bool>>? predicate = null;

            expressions.Add(_ => _.Recommendation.Recommender!.Id == recommender.Id);
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

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryARPOReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11)
        {
            DateTimeOffset startDate = dateTimeProvider.Now.ToUniversalTime().DateTimeSince(period, periodAgo);
            return await offerStore.QueryARPOReportData(campaign, period, startDate);
        }

        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryAPVReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11)
        {
            DateTimeOffset startDate = dateTimeProvider.Now.ToUniversalTime().DateTimeSince(period, periodAgo);
            return await offerStore.QueryAPVReportData(campaign, period, startDate);
        }
        public async Task<IEnumerable<OfferMeanGrossRevenue>> QueryPerformanceReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11)
        {
            // we can actually use the same data for this 
            return await QueryARPOReportData(campaign, period, periodAgo);
        }

        public async Task<IEnumerable<OfferConversionRateData>> QueryConversionRateReportData(PromotionsCampaign campaign, DateTimePeriod period, int periodAgo = 11)
        {
            DateTimeOffset startDate = dateTimeProvider.Now.ToUniversalTime().DateTimeSince(period, periodAgo);
            return await offerStore.QueryConversionRateData(campaign, period, startDate);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
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

        public async Task RedeemOffer(CustomerEvent customerEvent)
        {
            long? promotionId = TryGetValue<long>(customerEvent.Properties, "promotionId");

            // Only process Purchase and UsePromotion events with related recommendation and promotion
            if ((customerEvent.EventKind != EventKinds.Purchase &&
                customerEvent.EventKind != EventKinds.UsePromotion) ||
                !customerEvent.RecommendationCorrelatorId.HasValue ||
                !promotionId.HasValue)
            {
                return;
            }

            float? value = TryGetValue<float>(customerEvent.Properties, "value");
            var promotion = await promotionStore.Read(promotionId.Value);
            var result = await offerStore.ReadOfferByRecommendationCorrelator(customerEvent.RecommendationCorrelatorId.Value);
            if (result.Success)
            {
                var now = dateTimeProvider.Now;
                var offer = result.Entity;
                offer.State = OfferState.Redeemed;
                offer.LastUpdated = now;
                offer.RedeemedAt = now;
                offer.RedeemedPromotionId = promotion.Id;
                offer.RedeemedPromotion = promotion;
                if (customerEvent.EventKind == EventKinds.Purchase && value.HasValue)
                {
                    offer.GrossRevenue = value;
                }

                await offerStore.Update(offer);
                // Do not call Context.SaveChanges() for better performance. 
                // Calling method should handle this.
                logger.LogInformation("Offer {offerId} redeemed using promotion {promotionId}", offer.Id, promotion.Id);
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

        public async Task<Paginated<Offer>> QueryOffers(ItemsRecommender recommender, IPaginate paginate, OfferState? state = null)
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
    }
}
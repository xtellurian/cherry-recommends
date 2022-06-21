using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Campaigns;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Serialization;

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
        private readonly IStoreCollection storeCollection;

        public OfferWorkflows(
            ILogger<OfferWorkflows> logger,
            IOfferStore offerStore,
            IRecommendableItemStore promotionStore,
            IItemsRecommendationStore promotionsRecommendationStore,
            IDateTimeProvider dateTimeProvider,
            IStoreCollection storeCollection)
        {
            this.logger = logger;
            this.offerStore = offerStore;
            this.promotionStore = promotionStore;
            this.promotionsRecommendationStore = promotionsRecommendationStore;
            this.dateTimeProvider = dateTimeProvider;
            this.storeCollection = storeCollection;
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
            bool isShopifyEvent = customerEvent.EventKind == EventKinds.Custom && customerEvent.EventType == ShopifyConverter.OrdersPaidEvent;
            var result = await TryLoadOffer(customerEvent);
            var now = dateTimeProvider.Now;

            if (result.Success && result.Entity != null)
            {
                var offer = result.Entity;

                bool isPresentedEvent = customerEvent.EventKind == EventKinds.PromotionPresented && offer.State == OfferState.Created;
                bool isRedeemEvent = (customerEvent.EventKind == EventKinds.Purchase ||
                   customerEvent.EventKind == EventKinds.UsePromotion) && offer.State != OfferState.Redeemed;

                if (isPresentedEvent)
                {
                    offer.State = OfferState.Presented;
                    offer.LastUpdated = now;
                    logger.LogInformation("Offer {offerId} presented", offer.Id);
                }
                else if (isRedeemEvent)
                {
                    long? promotionId = TryGetValue<long>(customerEvent.Properties, "promotionId");
                    if (promotionId.HasValue)
                    {
                        if (offer.Recommendation == null)
                        {
                            await offerStore.Load(offer, _ => _.Recommendation);
                        }

                        // if the promotion is not part of the recommendation then ignore the event
                        if (offer.Recommendation != null)
                        {
                            await promotionsRecommendationStore.LoadMany(offer.Recommendation, _ => _.Items);
                            if (!offer.Recommendation.Items.Any(_ => _.Id == promotionId.Value))
                                return;
                        }

                        var promotion = await promotionStore.Read(promotionId.Value);
                        offer.RedeemedPromotionId = promotion.Id;
                        offer.RedeemedPromotion = promotion;
                    }
                    offer.State = OfferState.Redeemed;
                    offer.RedeemedCount++;
                    offer.LastUpdated = now;
                    offer.RedeemedAt = now;

                    if (customerEvent.EventKind == EventKinds.Purchase)
                    {
                        double? value = TryGetValue<double>(customerEvent.Properties, "value");
                        offer.GrossRevenue = value > 0 ? value : 1;
                    }
                    logger.LogInformation("Offer {offerId} redeemed using promotion {promotionId}", offer.Id, promotionId);
                }

                await offerStore.Update(offer);
                // Do not call Context.SaveChanges() for better performance. 
                // Calling method should handle this.
            }
            else if (isShopifyEvent)
            {
                // If a cherry discount code was used in a Shopify transaction then redeem offer to track revenue.
                if (customerEvent.Properties.ContainsKey("discountCodes"))
                {
                    var discountCodeStore = storeCollection.ResolveStore<IDiscountCodeStore, DiscountCode>();
                    IEnumerable<ShopifyDiscountCode> shopifyDiscountCodes = Enumerable.Empty<ShopifyDiscountCode>();

                    if (customerEvent.Properties["discountCodes"] is JsonElement je)
                    {
                        // case when event comes from event hub
                        var json = je.GetRawText();
                        var obj = Serializer.Deserialize<IEnumerable<ShopifyDiscountCode>>(json);
                        if (obj != null)
                        {
                            shopifyDiscountCodes = obj;
                        }
                    }
                    else if (customerEvent.Properties["discountCodes"] is IEnumerable<ShopifyDiscountCode> v)
                    {
                        // case when directly processing the event
                        shopifyDiscountCodes = v;
                    }

                    foreach (var shopifyDiscountCode in shopifyDiscountCodes)
                    {
                        var discountCodeResult = await discountCodeStore.ReadByCode(shopifyDiscountCode.Code);

                        if (discountCodeResult.Success && discountCodeResult.Entity != null)
                        {
                            var customerEventStore = storeCollection.ResolveStore<ICustomerEventStore, CustomerEvent>();
                            CustomerEvent existingEvent;
                            try
                            {
                                existingEvent = await customerEventStore.Read(customerEvent.EventId);
                                // Do not process existing events
                                // Shopify post events multiple times for delivery reliability
                                return;
                            }
                            catch
                            { }
                            var discountCode = discountCodeResult.Entity;
                            await discountCodeStore.Load(discountCode, _ => _.Promotion);
                            await discountCodeStore.LoadMany(discountCode, _ => _.Recommendations);
                            /* previously, we allowed discount code sharing between recommendations
                            which is why we have a many-to-many relationship. In practice, only one
                            discount code per recommendation is made. */
                            foreach (var recommendation in discountCode.Recommendations)
                            {
                                var offerResult = await offerStore.ReadOfferByRecommendation(recommendation);
                                if (offerResult.Success && offerResult.Entity != null)
                                {
                                    var offer = offerResult.Entity;
                                    var value = TryGetValue<double>(customerEvent.Properties, "totalPrice");
                                    offer.GrossRevenue += value ?? 0;
                                    offer.RedeemedPromotionId = discountCode.PromotionId;
                                    offer.RedeemedPromotion = discountCode.Promotion;
                                    offer.State = OfferState.Redeemed;
                                    offer.RedeemedCount++;
                                    offer.LastUpdated = now;
                                    offer.RedeemedAt = now;
                                    await offerStore.Update(offer);
                                    logger.LogInformation("Offer {offerId} redeemed using promotion {promotionId} with discount code {discountCode}", offer.Id, discountCode.PromotionId, discountCode.Code);
                                }
                            }
                        }
                    }
                }
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

        public async Task<IEnumerable<OfferSensitivityCurveData>> QuerySensitivityCurveReportData(PromotionsCampaign campaign, DateTimeOffset startDate)
        {
            return await offerStore.QueryOfferSensitivityCurveData(campaign, startDate);
        }
    }
}

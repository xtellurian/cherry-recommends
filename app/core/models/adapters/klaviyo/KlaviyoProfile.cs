using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Adapters.Klaviyo
{
    public class KlaviyoProfileResponse
    {
        /// <summary>
        /// Klaviyo Profile Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Klaviyo Profile Email
        /// </summary>
        public string Email { get; set; }
    }

    public class KlaviyoProfileCollection
    {
        public List<KlaviyoProfileRequest> Profiles { get; set; } = new List<KlaviyoProfileRequest>();
    }

    public class KlaviyoProfileRequest
    {
        public KlaviyoProfileRequest() { }

        public KlaviyoProfileRequest(ItemsRecommendation recommendation)
        {
            // add customer details
            Email = recommendation.Customer?.Email;
            Name = recommendation.Customer?.Name;

            // add promotion details
            if (recommendation.Items.Any())
            {
                var promoItem = recommendation.Items.First();

                PromotionCommonId = promoItem.CommonId;
                PromotionName = promoItem.Name;
                PromotionDirectCost = promoItem.DirectCost;
                PromotionDescription = promoItem.Description;
                PromotionBenefitType = promoItem.BenefitType.ToString();
                PromotionBenefitValue = promoItem.BenefitValue;
                PromotionType = promoItem.PromotionType.ToString();
                NumberOfRedemptions = promoItem.NumberOfRedemptions;
            }

            // add discount details
            if (recommendation.DiscountCodes.Any())
            {
                var discount = recommendation.DiscountCodes.First();

                DiscountCode = discount.Code;
                DiscountStartsAt = discount.StartsAt;
                DiscountEndsAt = discount.EndsAt;
            }
        }

        public string Email { get; set; }

        /// <summary>
        /// Klaviyo profile first name
        /// </summary>
        [JsonPropertyName("first_name")]
        public string Name { get; set; }

        [JsonPropertyName("promotion.name")]
        public string PromotionName { get; set; }

        [JsonPropertyName("promotion.commonId")]
        public string PromotionCommonId { get; set; }

#nullable enable
        // Recommendable Item
        [JsonPropertyName("promotion.directCost")]
        public double? PromotionDirectCost { get; set; }

        [JsonPropertyName("promotion.description")]
        public string PromotionDescription { get; set; }

        /// <summary>
        /// Percent / Fixed
        /// </summary>
        [JsonPropertyName("promotion.benefitType")]
        public string PromotionBenefitType { get; set; }

        [JsonPropertyName("promotion.benefitValue")]
        public double? PromotionBenefitValue { get; set; }

        /// <summary>
        /// Discount, Gift, Service, Upgrade or Other
        /// </summary>
        [JsonPropertyName("promotion.promotionType")]
        public string PromotionType { get; set; }

        [JsonPropertyName("promotion.numberOfRedemptions")]
        public int? NumberOfRedemptions { get; set; }

        // Discount Code - follow Klaviyo's property name
        [JsonPropertyName("coupon_code")]
        public string DiscountCode { get; set; }

        [JsonPropertyName("promotion.discountStartsAt")]
        public DateTimeOffset? DiscountStartsAt { get; set; }

        [JsonPropertyName("promotion.discountEndsAt")]
        public DateTimeOffset? DiscountEndsAt { get; set; }
    }
}
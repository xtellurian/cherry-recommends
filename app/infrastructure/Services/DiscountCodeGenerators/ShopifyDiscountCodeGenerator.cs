using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Infrastructure.Services
{
    public class ShopifyDiscountCodeGenerator : IDiscountCodeGenerator
    {
        private readonly ILogger<ShopifyDiscountCodeGenerator> logger;
        private readonly IShopifyService shopifyService;
        private readonly IShopifyAdminWorkflow shopifyAdminWorkflow;

        public ShopifyDiscountCodeGenerator(
            ILogger<ShopifyDiscountCodeGenerator> logger,
            IShopifyService shopifyService,
            IShopifyAdminWorkflow shopifyAdminWorkflow)
        {
            this.logger = logger;
            this.shopifyService = shopifyService;
            this.shopifyAdminWorkflow = shopifyAdminWorkflow;
        }

        public IntegratedSystemTypes SystemType => IntegratedSystemTypes.Shopify;

        public async Task Generate(IntegratedSystem system, RecommendableItem promotion, DiscountCode discountCode)
        {
            if (system == null)
            {
                throw new ArgumentNullException(nameof(system));
            }

            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion));
            }

            if (SystemType != system.SystemType)
            {
                throw new InvalidOperationException("System type mismatch.");
            }

            // Generate only if the integration status is OK and if it is a discount promotion
            if (system.IntegrationStatus != IntegrationStatuses.OK ||
                promotion.PromotionType != PromotionType.Discount ||
                promotion.BenefitValue == 0)
            {
                return;
            }

            var shop = shopifyAdminWorkflow.GetShopifyUrl(system);
            var accessToken = shopifyAdminWorkflow.GetAccessToken(system);

            // https://shopify.dev/api/admin-rest/2022-04/resources/pricerule#post-price-rules
            // Important thing to note here is that the generator does not track the price rules generated on Shopify.
            // Only the discount code is tracked in our system.
            try
            {
                int usageLimit = promotion.NumberOfRedemptions > 0 ? promotion.NumberOfRedemptions : 1;
                ShopifyPriceRule shopifyPriceRule = new ShopifyPriceRule
                {
                    Title = discountCode.Code, // For a consistent search experience, use the same value for title as the code property of the associated discount code.
                    ValueType = GetShopifyValueType(promotion.BenefitType), // fixed_amount | percentage
                    Value = (decimal)-Math.Abs(promotion.BenefitValue), // Must be a negative value
                    CustomerSelection = "all", // all | prerequisite (member of customer_segment_prerequisite_ids or prerequisite_customer_ids)
                    TargetSelection = "all", // all | entitled
                    TargetType = "line_item", // line_item | shipping_line
                    AllocationMethod = "across", // across | each
                    UsageLimit = usageLimit,
                    OncePerCustomer = false,
                    StartsAt = discountCode.StartsAt,
                    EndsAt = discountCode.EndsAt
                };

                shopifyPriceRule = await shopifyService.CreatePriceRule(shop, accessToken, shopifyPriceRule);

                var shopifyDiscountCode = new ShopifyPriceRuleDiscountCode
                {
                    Code = discountCode.Code,
                    PriceRuleId = shopifyPriceRule.Id
                };

                shopifyDiscountCode = await shopifyService.CreateDiscountCode(shop, accessToken, shopifyPriceRule.Id!.Value, shopifyDiscountCode);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Discount code creation failed for integrated system {integratedSystemId} with type {systemType}.", system.Id, system.SystemType);
                throw new IntegratedSystemException("Failed to create Shopify price rule and discount code for integrated system {integratedSystemId} with type {systemType}.", ex);
            }
        }

        public string GetShopifyValueType(BenefitType benefitType)
        {
            switch (benefitType)
            {
                case BenefitType.Fixed:
                    return "fixed_amount";
                case BenefitType.Percent:
                    return "percentage";
                default:
                    return string.Empty;
            }
        }
    }
}
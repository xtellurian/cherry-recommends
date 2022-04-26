
using ShopifySharp;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Infrastructure
{
    public static class ShopifyExtensions
    {
        public static ShopifyShop ToCoreRepresentation(this Shop _)
        {
            return new ShopifyShop
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                Address1 = _.Address1,
                Address2 = _.Address2,
                CheckoutApiSupported = _.CheckoutApiSupported,
                City = _.City,
                Country = _.Country,
                CountryCode = _.CountryCode,
                CountryName = _.CountryName,
                CountyTaxes = _.CountyTaxes,
                CreatedAt = _.CreatedAt,
                Currency = _.Currency,
                CustomerEmail = _.CustomerEmail,
                Description = _.Description,
                Domain = _.Domain,
                EligibleForCardReaderGiveaway = _.EligibleForCardReaderGiveaway,
                EligibleForPayments = _.EligibleForPayments,
                Email = _.Email,
                EnabledPresentmentCurrencies = _.EnabledPresentmentCurrencies,
                GoogleAppsDomain = _.GoogleAppsDomain,
                GoogleAppsLoginEnabled = _.GoogleAppsLoginEnabled,
                HasDiscounts = _.HasDiscounts,
                HasGiftCards = _.HasGiftCards,
                HasStorefront = _.HasStorefront,
                IANATimezone = _.IANATimezone,
                Latitude = _.Latitude,
                Longitude = _.Longitude,
                MoneyFormat = _.MoneyFormat,
                MoneyInEmailsFormat = _.MoneyInEmailsFormat,
                MoneyWithCurrencyFormat = _.MoneyWithCurrencyFormat,
                MoneyWithCurrencyInEmailsFormat = _.MoneyWithCurrencyInEmailsFormat,
                MultiLocationEnabled = _.MultiLocationEnabled,
                MyShopifyDomain = _.MyShopifyDomain,
                Name = _.Name,
                PasswordEnabled = _.PasswordEnabled,
                Phone = _.Phone,
                PlanDisplayName = _.PlanDisplayName,
                PlanName = _.PlanName,
                PreLaunchEnabled = _.PreLaunchEnabled,
                PrimaryLocale = _.PrimaryLocale,
                PrimaryLocationId = _.PrimaryLocationId,
                Province = _.Province,
                ProvinceCode = _.ProvinceCode,
                RequiresExtraPaymentsAgreement = _.RequiresExtraPaymentsAgreement,
                SetupRequired = _.SetupRequired,
                ShipsToCountries = _.ShipsToCountries,
                ShopOwner = _.ShopOwner,
                Source = _.Source,
                TaxesIncluded = _.TaxesIncluded,
                TaxShipping = _.TaxShipping,
                Timezone = _.Timezone,
                UpdatedAt = _.UpdatedAt,
                WeightUnit = _.WeightUnit
            };
        }

        public static Shop ToShopifySharpRepresentation(ShopifyShop _)
        {
            return new Shop
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                Address1 = _.Address1,
                Address2 = _.Address2,
                CheckoutApiSupported = _.CheckoutApiSupported,
                City = _.City,
                Country = _.Country,
                CountryCode = _.CountryCode,
                CountryName = _.CountryName,
                CountyTaxes = _.CountyTaxes,
                CreatedAt = _.CreatedAt,
                Currency = _.Currency,
                CustomerEmail = _.CustomerEmail,
                Description = _.Description,
                Domain = _.Domain,
                EligibleForCardReaderGiveaway = _.EligibleForCardReaderGiveaway,
                EligibleForPayments = _.EligibleForPayments,
                Email = _.Email,
                EnabledPresentmentCurrencies = _.EnabledPresentmentCurrencies,
                GoogleAppsDomain = _.GoogleAppsDomain,
                GoogleAppsLoginEnabled = _.GoogleAppsLoginEnabled,
                HasDiscounts = _.HasDiscounts,
                HasGiftCards = _.HasGiftCards,
                HasStorefront = _.HasStorefront,
                IANATimezone = _.IANATimezone,
                // Latitude = _.Latitude,
                // Longitude = _.Longitude,
                MoneyFormat = _.MoneyFormat,
                MoneyInEmailsFormat = _.MoneyInEmailsFormat,
                MoneyWithCurrencyFormat = _.MoneyWithCurrencyFormat,
                MoneyWithCurrencyInEmailsFormat = _.MoneyWithCurrencyInEmailsFormat,
                MultiLocationEnabled = _.MultiLocationEnabled,
                MyShopifyDomain = _.MyShopifyDomain,
                Name = _.Name,
                PasswordEnabled = _.PasswordEnabled,
                Phone = _.Phone,
                PlanDisplayName = _.PlanDisplayName,
                PlanName = _.PlanName,
                PreLaunchEnabled = _.PreLaunchEnabled,
                PrimaryLocale = _.PrimaryLocale,
                PrimaryLocationId = _.PrimaryLocationId,
                Province = _.Province,
                ProvinceCode = _.ProvinceCode,
                RequiresExtraPaymentsAgreement = _.RequiresExtraPaymentsAgreement,
                SetupRequired = _.SetupRequired,
                ShipsToCountries = _.ShipsToCountries,
                ShopOwner = _.ShopOwner,
                Source = _.Source,
                TaxesIncluded = _.TaxesIncluded,
                TaxShipping = _.TaxShipping,
                Timezone = _.Timezone,
                UpdatedAt = _.UpdatedAt,
                WeightUnit = _.WeightUnit
            };
        }

        public static ShopifyWebhook ToCoreRepresentation(this Webhook _)
        {
            return new ShopifyWebhook
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                Address = _.Address,
                CreatedAt = _.CreatedAt,
                Fields = _.Fields,
                Format = _.Format,
                MetafieldNamespaces = _.MetafieldNamespaces,
                Topic = _.Topic,
                UpdatedAt = _.UpdatedAt
            };
        }

        public static Webhook ToShopifySharpRepresentation(this ShopifyWebhook _)
        {
            return new Webhook
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                Address = _.Address,
                CreatedAt = _.CreatedAt,
                Fields = _.Fields,
                Format = _.Format,
                MetafieldNamespaces = _.MetafieldNamespaces,
                Topic = _.Topic,
                UpdatedAt = _.UpdatedAt
            };
        }

        public static ShopifyPriceRule ToCoreRepresentation(this PriceRule _)
        {
            return new ShopifyPriceRule
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                AllocationMethod = _.AllocationMethod,
                CreatedAt = _.CreatedAt,
                CustomerSelection = _.CustomerSelection,
                EndsAt = _.EndsAt,
                EntitledCollectionIds = _.EntitledCollectionIds,
                EntitledCountryIds = _.EntitledCountryIds,
                EntitledProductIds = _.EntitledProductIds,
                EntitledVariantIds = _.EntitledVariantIds,
                OncePerCustomer = _.OncePerCustomer,
                PrerequisiteCustomerIds = _.PrerequisiteCustomerIds,
                PrerequisiteSavedSearchIds = _.PrerequisiteSavedSearchIds,
                PrerequisiteShippingPriceRange = _.PrerequisiteShippingPriceRange != null
                ? new Core.Adapters.Shopify.PrerequisiteValueRange
                {
                    GreaterThanOrEqualTo = _.PrerequisiteShippingPriceRange.GreaterThanOrEqualTo,
                    LessThanOrEqualTo = _.PrerequisiteShippingPriceRange.LessThanOrEqualTo
                } : null,
                PrerequisiteSubtotalRange = _.PrerequisiteSubtotalRange != null
                ? new Core.Adapters.Shopify.PrerequisiteValueRange
                {
                    GreaterThanOrEqualTo = _.PrerequisiteShippingPriceRange.GreaterThanOrEqualTo,
                    LessThanOrEqualTo = _.PrerequisiteShippingPriceRange.LessThanOrEqualTo
                } : null,
                StartsAt = _.StartsAt,
                TargetSelection = _.TargetSelection,
                TargetType = _.TargetType,
                Title = _.Title,
                UpdatedAt = _.UpdatedAt,
                UsageLimit = _.UsageLimit,
                Value = _.Value,
                ValueType = _.ValueType
            };
        }

        public static PriceRule ToShopifySharpRepresentation(this ShopifyPriceRule _)
        {
            return new PriceRule
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                AllocationMethod = _.AllocationMethod,
                CreatedAt = _.CreatedAt,
                CustomerSelection = _.CustomerSelection,
                EndsAt = _.EndsAt,
                EntitledCollectionIds = _.EntitledCollectionIds,
                EntitledCountryIds = _.EntitledCountryIds,
                EntitledProductIds = _.EntitledProductIds,
                EntitledVariantIds = _.EntitledVariantIds,
                OncePerCustomer = _.OncePerCustomer,
                PrerequisiteCustomerIds = _.PrerequisiteCustomerIds,
                PrerequisiteSavedSearchIds = _.PrerequisiteSavedSearchIds,
                PrerequisiteShippingPriceRange = _.PrerequisiteShippingPriceRange != null
                ? new ShopifySharp.PrerequisiteValueRange
                {
                    GreaterThanOrEqualTo = _.PrerequisiteShippingPriceRange.GreaterThanOrEqualTo,
                    LessThanOrEqualTo = _.PrerequisiteShippingPriceRange.LessThanOrEqualTo
                } : null,
                PrerequisiteSubtotalRange = _.PrerequisiteSubtotalRange != null
                ? new ShopifySharp.PrerequisiteValueRange
                {
                    GreaterThanOrEqualTo = _.PrerequisiteShippingPriceRange.GreaterThanOrEqualTo,
                    LessThanOrEqualTo = _.PrerequisiteShippingPriceRange.LessThanOrEqualTo
                } : null,
                StartsAt = _.StartsAt,
                TargetSelection = _.TargetSelection,
                TargetType = _.TargetType,
                Title = _.Title,
                UpdatedAt = _.UpdatedAt,
                UsageLimit = _.UsageLimit,
                Value = _.Value,
                ValueType = _.ValueType
            };
        }

        public static ShopifyPriceRuleDiscountCode ToCoreRepresentation(this PriceRuleDiscountCode _)
        {
            return new ShopifyPriceRuleDiscountCode
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                Code = _.Code,
                CreatedAt = _.CreatedAt,
                PriceRuleId = _.PriceRuleId,
                UpdatedAt = _.UpdatedAt,
                UsageCount = _.UsageCount
            };
        }

        public static PriceRuleDiscountCode ToShopifySharpRepresentation(this ShopifyPriceRuleDiscountCode _)
        {
            return new PriceRuleDiscountCode
            {
                Id = _.Id,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                Code = _.Code,
                CreatedAt = _.CreatedAt,
                PriceRuleId = _.PriceRuleId,
                UpdatedAt = _.UpdatedAt,
                UsageCount = _.UsageCount
            };
        }

        // recurring charges
        public static ShopifyRecurringCharge ToCoreRepresentation(this RecurringCharge _)
        {
            return new ShopifyRecurringCharge
            {
                Id = _.Id,
                ActivatedOn = _.ActivatedOn,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                BillingOn = _.BillingOn,
                CreatedAt = _.CreatedAt,
                CancelledOn = _.CancelledOn,
                CappedAmount = _.CappedAmount,
                ConfirmationUrl = _.ConfirmationUrl,
                Name = _.Name,
                Price = _.Price,
                ReturnUrl = _.ReturnUrl,
                Status = _.Status,
                Terms = _.Terms,
                Test = _.Test,
                TrialDays = _.TrialDays,
                TrialEndsOn = _.TrialEndsOn,
                UpdatedAt = _.UpdatedAt,
            };
        }

        public static RecurringCharge ToShopifySharpRepresentation(this ShopifyRecurringCharge _)
        {
            return new RecurringCharge
            {
                Id = _.Id,
                ActivatedOn = _.ActivatedOn,
                AdminGraphQLAPIId = _.AdminGraphQLAPIId,
                BillingOn = _.BillingOn,
                CreatedAt = _.CreatedAt,
                CancelledOn = _.CancelledOn,
                CappedAmount = _.CappedAmount,
                ConfirmationUrl = _.ConfirmationUrl,
                Name = _.Name,
                Price = _.Price,
                ReturnUrl = _.ReturnUrl,
                Status = _.Status,
                Terms = _.Terms,
                Test = _.Test,
                TrialDays = _.TrialDays,
                TrialEndsOn = _.TrialEndsOn,
                UpdatedAt = _.UpdatedAt,
            };
        }
    }
}

using System;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class ShopifyShop : ShopifyObjectBase
    {
        [JsonPropertyName("pre_launch_enabled")]
        public bool? PreLaunchEnabled { get; set; }
        [JsonPropertyName("requires_extra_payments_agreement")]
        public bool? RequiresExtraPaymentsAgreement { get; set; }
        [JsonPropertyName("myshopify_domain")]
        public string MyShopifyDomain { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("plan_name")]
        public string PlanName { get; set; }
        [JsonPropertyName("plan_display_name")]
        public string PlanDisplayName { get; set; }
        [JsonPropertyName("password_enabled")]
        public bool? PasswordEnabled { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("primary_locale")]
        public string PrimaryLocale { get; set; }
        [JsonPropertyName("province")]
        public string Province { get; set; }
        [JsonPropertyName("province_code")]
        public string ProvinceCode { get; set; }
        [JsonPropertyName("ships_to_countries")]
        public string ShipsToCountries { get; set; }
        [JsonPropertyName("shop_owner")]
        public string ShopOwner { get; set; }
        [JsonPropertyName("source")]
        public string Source { get; set; }
        [JsonPropertyName("tax_shipping")]
        public bool? TaxShipping { get; set; }
        [JsonPropertyName("taxes_included")]
        public bool? TaxesIncluded { get; set; }
        [JsonPropertyName("county_taxes")]
        public bool? CountyTaxes { get; set; }
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("iana_timezone")]
        public string IANATimezone { get; set; }
        [JsonPropertyName("zip")]
        public string Zip { get; set; }
        [JsonPropertyName("has_storefront")]
        public bool? HasStorefront { get; set; }
        [JsonPropertyName("setup_required")]
        public bool? SetupRequired { get; set; }
        [JsonPropertyName("weight_unit")]
        public string WeightUnit { get; set; }
        [JsonPropertyName("multi_location_enabled")]
        public bool? MultiLocationEnabled { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
        [JsonPropertyName("money_with_currency_in_emails_format")]
        public string MoneyWithCurrencyInEmailsFormat { get; set; }
        [JsonPropertyName("money_in_emails_format")]
        public string MoneyInEmailsFormat { get; set; }
        [JsonPropertyName("address1")]
        public string Address1 { get; set; }
        [JsonPropertyName("address2")]
        public string Address2 { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("domain")]
        public string Domain { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("enabled_presentment_currencies")]
        public string[] EnabledPresentmentCurrencies { get; set; }
        [JsonPropertyName("google_apps_domain")]
        public string GoogleAppsDomain { get; set; }
        [JsonPropertyName("google_apps_login_enabled")]
        public string GoogleAppsLoginEnabled { get; set; }
        [JsonPropertyName("eligible_for_card_reader_giveaway")]
        public bool? EligibleForCardReaderGiveaway { get; set; }
        [JsonPropertyName("eligible_for_payments")]
        public bool? EligibleForPayments { get; set; }
        [JsonPropertyName("checkout_api_supported")]
        public bool? CheckoutApiSupported { get; set; }
        [JsonPropertyName("has_discounts")]
        public bool? HasDiscounts { get; set; }
        [JsonPropertyName("has_gift_cards")]
        public bool? HasGiftCards { get; set; }
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }
        [JsonPropertyName("money_format")]
        public string MoneyFormat { get; set; }
        [JsonPropertyName("money_with_currency_format")]
        public string MoneyWithCurrencyFormat { get; set; }
        [JsonPropertyName("primary_location_id")]
        public long? PrimaryLocationId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Core
{
    public interface IShopifyService
    {
        Task<Uri> BuildAuthorizationUrl(string shopifyUrl, string redirectUrl, string state);
        Task<string> Authorize(string code, string shopifyUrl);
        Task<bool> IsAuthenticRequest(IDictionary<string, string> queryString);
        Task<bool> IsAuthenticProxyRequest(IDictionary<string, string> queryString);
        Task<bool> IsAuthenticWebhook(IEnumerable<KeyValuePair<string, StringValues>> requestHeaders, string requestBody);
        Task<bool> IsValidShopDomainAsync(string shopifyUrl);
        Task<ShopifyShop> GetShopInformation(string shopifyUrl, string accessToken);
        Task<bool> UninstallApp(string shopifyUrl, string accessToken);
        Task<ShopifyWebhook> CreateWebhook(string shopifyUrl, string accessToken, string address, string topic, IEnumerable<string> fields = null, IEnumerable<string> metafieldNamespaces = null);
        Task<ShopifyPriceRule> CreatePriceRule(string shopifyUrl, string accessToken, ShopifyPriceRule priceRule);
        Task<IEnumerable<ShopifyPriceRule>> GetPriceRules(string shopifyUrl, string accessToken);
        Task<ShopifyPriceRule> GetPriceRule(string shopifyUrl, string accessToken, long priceRuleId);
        Task<ShopifyPriceRuleDiscountCode> CreateDiscountCode(string shopifyUrl, string accessToken, long priceRuleId, ShopifyPriceRuleDiscountCode discountCode);
        Task<ShopifyRecurringCharge> CreateRecurringCharge(string shopifyUrl, string accessToken, ShopifyRecurringCharge recurringCharge);
        Task<IEnumerable<ShopifyRecurringCharge>> ListRecurringCharges(string shopifyUrl, string accessToken);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using ShopifySharp;
using SignalBox.Core;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Integrations;

/// <summary>Uses the ShopifySharp library. Github: https://github.com/nozzlegear/ShopifySharp</summary>
namespace SignalBox.Infrastructure.Services
{
    public class ShopifyService : IShopifyService
    {
        private readonly ShopifyAppCredentials creds;
        private readonly ILogger<ShopifyService> logger;

        #region Authorization
        public ShopifyService(IOptions<ShopifyAppCredentials> creds, ILogger<ShopifyService> logger)
        {
            this.creds = creds.Value;
            this.logger = logger;
        }

        public Task<Uri> BuildAuthorizationUrl(string shopifyUrl, string redirectUrl, string state)
        {
            return Task.FromResult(AuthorizationService.BuildAuthorizationUrl(creds.Scopes, shopifyUrl, creds.ApiKey, redirectUrl, state));
        }

        public Task<string> Authorize(string code, string shopifyUrl)
        {
            return AuthorizationService.Authorize(code, shopifyUrl, creds.ApiKey, creds.SecretKey);
        }

        public Task<bool> IsAuthenticRequest(IDictionary<string, string> queryString)
        {
            return Task.FromResult(AuthorizationService.IsAuthenticRequest(queryString, creds.SecretKey));
        }

        public Task<bool> IsAuthenticProxyRequest(IDictionary<string, string> queryString)
        {
            return Task.FromResult(AuthorizationService.IsAuthenticProxyRequest(queryString, creds.SecretKey));
        }

        public Task<bool> IsAuthenticWebhook(IEnumerable<KeyValuePair<string, StringValues>> requestHeaders, string requestBody)
        {
            return Task.FromResult(AuthorizationService.IsAuthenticWebhook(requestHeaders, requestBody, creds.SecretKey));
        }

        public Task<bool> IsValidShopDomainAsync(string shopifyUrl)
        {
            return AuthorizationService.IsValidShopDomainAsync(shopifyUrl);
        }
        #endregion

        public async Task<ShopifyShop> GetShopInformation(string shopifyUrl, string accessToken)
        {
            var service = new ShopService(shopifyUrl, accessToken);
            var _ = await service.GetAsync();

            return _.ToCoreRepresentation();
        }

        public async Task<bool> UninstallApp(string shopifyUrl, string accessToken)
        {
            var service = new ShopService(shopifyUrl, accessToken);
            bool success = false;
            try
            {
                await service.UninstallAppAsync();
                success = true;
            }
            catch (Exception)
            { }

            return success;
        }

        public async Task<ShopifyWebhook> CreateWebhook(string shopifyUrl, string accessToken, string address, string topic, IEnumerable<string> fields = null, IEnumerable<string> metafieldNamespaces = null)
        {
            var service = new WebhookService(shopifyUrl, accessToken);
            var _ = new Webhook()
            {
                Address = address,
                CreatedAt = DateTime.Now,
                Fields = fields,
                Format = "json",
                MetafieldNamespaces = metafieldNamespaces,
                Topic = topic,
            };
            _ = await service.CreateAsync(_);

            return _.ToCoreRepresentation();
        }

        public async Task<ShopifyPriceRule> CreatePriceRule(string shopifyUrl, string accessToken, ShopifyPriceRule priceRule)
        {
            var service = new PriceRuleService(shopifyUrl, accessToken);
            var _ = priceRule.ToShopifySharpRepresentation();
            _ = await service.CreateAsync(_);

            return _.ToCoreRepresentation();
        }

        public async Task<IEnumerable<ShopifyPriceRule>> GetPriceRules(string shopifyUrl, string accessToken)
        {
            var service = new PriceRuleService(shopifyUrl, accessToken);
            var entities = await service.ListAsync();

            return entities.Items.Select(_ => _.ToCoreRepresentation());
        }

        public async Task<ShopifyPriceRule> GetPriceRule(string shopifyUrl, string accessToken, long priceRuleId)
        {
            var service = new PriceRuleService(shopifyUrl, accessToken);
            var _ = await service.GetAsync(priceRuleId);

            return _.ToCoreRepresentation();
        }

        public async Task<ShopifyPriceRuleDiscountCode> CreateDiscountCode(string shopifyUrl, string accessToken, long priceRuleId, ShopifyPriceRuleDiscountCode discountCode)
        {
            var service = new DiscountCodeService(shopifyUrl, accessToken);
            var _ = discountCode.ToShopifySharpRepresentation();
            _ = await service.CreateAsync(priceRuleId, _);

            return _.ToCoreRepresentation();
        }
    }
}
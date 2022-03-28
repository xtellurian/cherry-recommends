using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using ShopifySharp;
using SignalBox.Core;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Integrations;
using SignalBox.Core.Serialization;

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

        public Task<bool> IsAuthenticProxyRequest(IEnumerable<KeyValuePair<string, StringValues>> requestHeaders, Stream inputStream)
        {
            return AuthorizationService.IsAuthenticWebhook(requestHeaders, inputStream, creds.SecretKey);
        }

        public Task<bool> IsValidShopDomainAsync(string shopifyUrl)
        {
            return AuthorizationService.IsValidShopDomainAsync(shopifyUrl);
        }
        #endregion

        public async Task<ShopifyShop> GetShopInformation(string shopifyUrl, string accessToken)
        {
            var service = new ShopService(shopifyUrl, accessToken);
            var shop = await service.GetAsync();
            var shopifyShop = Serializer.Deserialize<ShopifyShop>(Serializer.Serialize(shop));

            return shopifyShop;
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
    }
}
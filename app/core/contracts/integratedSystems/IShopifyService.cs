using System;
using System.Collections.Generic;
using System.IO;
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
        Task<bool> IsAuthenticProxyRequest(IEnumerable<KeyValuePair<string, StringValues>> requestHeaders, Stream inputStream);
        Task<bool> IsValidShopDomainAsync(string shopifyUrl);
        Task<ShopifyShop> GetShopInformation(string shopifyUrl, string accessToken);
        Task<bool> UninstallApp(string shopifyUrl, string accessToken);
    }
}
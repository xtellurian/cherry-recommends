using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Integrations;

namespace SignalBox.Core
{
    public interface IShopifyWorkflowBase
    {
        Task<ShopifyAppCredentials> GetShopifyCredentials();
        Task<Uri> BuildAuthorizationUrl(string shopifyUrl, string redirectUrl, string state);
        Task Authorize(IntegratedSystem system, string code, string shopifyUrl);
        Task UninstallApp(IntegratedSystem system, bool errorOnUninstall = true);
    }
}
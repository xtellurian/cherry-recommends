using SignalBox.Core.Adapters.Hubspot;

namespace SignalBox.Core.Integrations
{
    public class ShopifyCache : IIntegratedSystemCache
    {
        public ShopifyCache(ShopifyStoreCredentials storeCredentials)
        {
            StoreCredentials = storeCredentials;
        }

        public ShopifyStoreCredentials StoreCredentials { get; }
    }
}
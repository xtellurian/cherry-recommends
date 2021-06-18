using SignalBox.Core.Adapters.Hubspot;

namespace SignalBox.Core.Integrations.Hubspot
{
    public class HubspotCache : IIntegratedSystemCache
    {
        public HubspotCache()
        {
        }

        public HubspotCache(HubspotAccountDetails accountDetails)
        {
            AccountDetails = accountDetails;
        }

        public HubspotAccountDetails AccountDetails { get; set; }
    }
}
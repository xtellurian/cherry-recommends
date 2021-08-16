using SignalBox.Core.Adapters.Hubspot;

#nullable enable
namespace SignalBox.Core.Integrations.Hubspot
{
    public class HubspotCache : IIntegratedSystemCache
    {
        public HubspotCache()
        { }

        public HubspotCache(HubspotAccountDetails accountDetails)
        {
            AccountDetails = accountDetails;
        }

        public HubspotAccountDetails? AccountDetails { get; set; }
        public HubspotWebhookBehaviour? WebhookBehaviour { get; set; }
        public FeatureCrmCardBehaviour? FeatureCrmCardBehaviour { get; set; }
    }
}
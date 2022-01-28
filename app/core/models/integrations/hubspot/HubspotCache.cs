using SignalBox.Core.Adapters.Hubspot;

#nullable enable
namespace SignalBox.Core.Integrations.Hubspot
{
    public class HubspotCache : IIntegratedSystemCache
    {
        private HubspotPropertyCollection? connectedContactProperties;

        public HubspotCache()
        { }

        public HubspotCache(HubspotAccountDetails accountDetails)
        {
            AccountDetails = accountDetails;
        }

        public HubspotAccountDetails? AccountDetails { get; set; }
        public HubspotTrackedUserLinkBehaviour? WebhookBehaviour { get; set; }
        public HubspotPropertyCollection? ConnectedContactProperties
        {
            get => connectedContactProperties; set
            {
                connectedContactProperties?.Validate();
                connectedContactProperties = value;
            }
        }
        public MetricCrmCardBehaviour? FeatureCrmCardBehaviour
        {
            get => MetricCrmCardBehaviour; set
            {
                if (value != null)
                {
                    MetricCrmCardBehaviour = value;
                }
            }
        }
        public MetricCrmCardBehaviour? MetricCrmCardBehaviour { get; set; }
        public PushBehaviour? PushBehaviour { get; set; }
    }
}
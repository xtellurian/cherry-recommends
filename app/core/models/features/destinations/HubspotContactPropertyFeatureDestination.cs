using System.Collections.Generic;

#nullable enable
namespace SignalBox.Core.Features.Destinations
{
    public class HubspotContactPropertyFeatureDestination : FeatureDestinationBase
    {
        protected HubspotContactPropertyFeatureDestination()
        {
            HubspotPropertyName = null!;
        }

        public HubspotContactPropertyFeatureDestination(Feature feature, IntegratedSystem connectedSystem, string hubspotPropertyName)
         : base(feature, connectedSystem)
        {

            this.HubspotPropertyName = hubspotPropertyName;
        }

        public override string DestinationType => HubspotContactPropertyDestinationType;
        public string HubspotPropertyName { get; set; }

        public override IDictionary<string, string> Properties =>
        new Dictionary<string, string>
        {
            { "propertyName", HubspotPropertyName }
        };

        public string? ApplicationSecret => null;
    }
}
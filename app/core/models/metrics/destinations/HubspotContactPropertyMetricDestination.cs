using System.Collections.Generic;

#nullable enable
namespace SignalBox.Core.Metrics.Destinations
{
    public class HubspotContactPropertyMetricDestination : MetricDestinationBase
    {
        protected HubspotContactPropertyMetricDestination()
        {
            HubspotPropertyName = null!;
        }

        public HubspotContactPropertyMetricDestination(Metric metric, IntegratedSystem connectedSystem, string hubspotPropertyName)
         : base(metric, connectedSystem)
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
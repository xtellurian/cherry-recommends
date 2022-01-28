using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Core.Metrics.Destinations
{
#nullable enable
    public abstract class MetricDestinationBase : Entity, IHierarchyBase
    {
        public const string WebhookDestinationType = "Webhook";
        public const string HubspotContactPropertyDestinationType = "HubspotContactProperty";
        protected MetricDestinationBase()
        {
            Metric = null!;
            ConnectedSystem = null!;
        }

        public MetricDestinationBase(Metric metric, IntegratedSystem connectedSystem)
        {
            Metric = metric;
            ConnectedSystem = connectedSystem;
            ConnectedSystemId = connectedSystem.Id;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();
            if (this.ConnectedSystem is CustomIntegratedSystem)
            {
                if (this is WebhookMetricDestination)
                { }
                else
                {
                    throw new BadRequestException("A custom system integration can only have a Webhook Destination");
                }
            }
            else if (this.ConnectedSystem.SystemType == IntegratedSystemTypes.Hubspot)
            {
                if (this is HubspotContactPropertyMetricDestination)
                { }
                else
                {
                    throw new BadRequestException("A Hubspot system integration can only have a Hubspot Contact Property Destination");
                }
            }
            else
            {
                throw new BadRequestException("Bad destination configuraton.");
            }
        }

        public abstract IDictionary<string, string>? Properties { get; }
        public abstract string DestinationType { get; }
        public Metric Metric { get; set; }
        public Metric Feature => Metric;
        public virtual long ConnectedSystemId { get; set; }
        public virtual IntegratedSystem ConnectedSystem { get; set; }
        public string? Discriminator { get; set; }
    }

}
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Core.Features.Destinations
{
#nullable enable
    public abstract class FeatureDestinationBase : Entity, IHierarchyBase
    {
        public const string WebhookDestinationType = "Webhook";
        public const string HubspotContactPropertyDestinationType = "HubspotContactProperty";
        protected FeatureDestinationBase()
        {
            Feature = null!;
            ConnectedSystem = null!;
        }

        public FeatureDestinationBase(Feature feature, IntegratedSystem connectedSystem)
        {
            Feature = feature;
            ConnectedSystem = connectedSystem;
            ConnectedSystemId = connectedSystem.Id;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();
            if (this.ConnectedSystem is CustomIntegratedSystem)
            {
                if (this is WebhookFeatureDestination)
                { }
                else
                {
                    throw new BadRequestException("A custom system integration can only have a Webhook Destination");
                }
            }
            else if (this.ConnectedSystem.SystemType == IntegratedSystemTypes.Hubspot)
            {
                if (this is HubspotContactPropertyFeatureDestination)
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
        public Feature Feature { get; set; }
        public virtual long ConnectedSystemId { get; set; }
        public virtual IntegratedSystem ConnectedSystem { get; set; }
        public string? Discriminator { get; set; }
    }

}
using System.Collections.Generic;
using SignalBox.Core.Integrations.Custom;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations.Destinations
{
#nullable enable
    public abstract class RecommendationDestinationBase : EnvironmentScopedEntity, IHierarchyBase
    {
        public const string WebhookDestinationType = "Webhook";
        public const string SegmentSourceFunctionDestinationType = "SegmentSourceFunction";
        protected RecommendationDestinationBase()
        {
            Recommender = null!;
            ConnectedSystem = null!;
        }

        public RecommendationDestinationBase(RecommenderEntityBase recommender, IntegratedSystem connectedSystem)
        {
            Recommender = recommender;
            ConnectedSystem = connectedSystem;
            ConnectedSystemId = connectedSystem.Id;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();
            if (this.ConnectedSystem is CustomIntegratedSystem)
            {
                if (this is WebhookDestination)
                { }
                else
                {
                    throw new BadRequestException("A custom system integration can only have a Webhook Destination");
                }
            }
            if (this.ConnectedSystem.SystemType == IntegratedSystemTypes.Segment)
            {
                if (this is SegmentSourceFunctionDestination)
                { }
                else
                {
                    throw new BadRequestException("A Segment system integration can only have a Segment Source Function Destination");
                }
            }
        }

        public abstract IDictionary<string, string>? Properties { get; }
        public abstract string DestinationType { get; }
        public RecommenderEntityBase Recommender { get; set; }
        public DestinationTrigger? Trigger { get; set; }

        public virtual long ConnectedSystemId { get; set; }
        public virtual IntegratedSystem ConnectedSystem { get; set; }
        public string? Discriminator { get; set; }
    }

}
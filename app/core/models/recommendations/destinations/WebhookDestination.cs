using System.Collections.Generic;
using SignalBox.Core.Integrations.Custom;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Recommendations.Destinations
{
    public class WebhookDestination : RecommendationDestinationBase, IWebhookDestination
    {
        protected WebhookDestination()
        {
            Endpoint = null!;
        }
        public WebhookDestination(RecommenderEntityBase recommender, IntegratedSystem connectedSystem, string endpoint)
         : base(recommender, connectedSystem)
        {

            if (System.Uri.TryCreate(endpoint, System.UriKind.Absolute, out var u))
            {
                this.Endpoint = u.ToString();
            }
            else
            {
                throw new BadRequestException($"Endpoint {endpoint} is badly formatted");
            }
            this.EnvironmentId = connectedSystem.EnvironmentId;
        }

        public string? ApplicationSecret => (this.ConnectedSystem as CustomIntegratedSystem)?.ApplicationSecret;

        public override string DestinationType => WebhookDestinationType;
        public string Endpoint { get; set; }

        public override IDictionary<string, string> Properties =>
        new Dictionary<string, string>
        {
            {"endpoint", Endpoint}
        };
    }
}
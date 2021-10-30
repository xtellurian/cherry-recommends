using System.Collections.Generic;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations.Destinations
{
    public class WebhookDestination : RecommendationDestinationBase, IWebhookDestination
    {
        protected WebhookDestination() { }
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
        }

        public override string DestinationType => WebhookDestinationType;
        public string Endpoint { get; set; }

        public override IDictionary<string, string> Properties =>
        new Dictionary<string, string>
        {
            {"endpoint", Endpoint}
        };
    }
}
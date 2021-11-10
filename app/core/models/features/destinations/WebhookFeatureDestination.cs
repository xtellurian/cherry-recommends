using System.Collections.Generic;
using SignalBox.Core.Integrations.Custom;

#nullable enable
namespace SignalBox.Core.Features.Destinations
{
    public class WebhookFeatureDestination : FeatureDestinationBase, IWebhookDestination
    {
        protected WebhookFeatureDestination()
        {
            Endpoint = null!;
        }
        public WebhookFeatureDestination(Feature feature, IntegratedSystem connectedSystem, string endpoint)
         : base(feature, connectedSystem)
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
using System.Collections.Generic;
using SignalBox.Core.Integrations.Custom;

#nullable enable
namespace SignalBox.Core.Metrics.Destinations
{
    public class WebhookMetricDestination : MetricDestinationBase, IWebhookDestination
    {
        protected WebhookMetricDestination()
        {
            Endpoint = null!;
        }
        public WebhookMetricDestination(Metric metric, IntegratedSystem connectedSystem, string endpoint)
         : base(metric, connectedSystem)
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
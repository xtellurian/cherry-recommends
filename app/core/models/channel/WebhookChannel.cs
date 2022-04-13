using System.Collections.Generic;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Core
{

    public class WebhookChannel : ChannelBase, IWebhookDestination
    {
        protected WebhookChannel()
        { }

        public WebhookChannel(string name, IntegratedSystem linkedSystem)
            : base(name, ChannelTypes.Webhook, linkedSystem)
        {
        }

        public string Endpoint { get; set; }
        public override IDictionary<string, object> Properties =>
        new Dictionary<string, object>
        {
            {"endpoint", Endpoint}
        };
#nullable enable
        public string? ApplicationSecret => (this.LinkedIntegratedSystem as CustomIntegratedSystem)?.ApplicationSecret;
    }
}

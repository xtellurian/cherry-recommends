using System.Collections.Generic;
using SignalBox.Core.Integrations.Website;

namespace SignalBox.Core
{

    public class WebChannel : ChannelBase, IWebhookDestination
    {
        protected WebChannel()
        { }

        public WebChannel(string name, IntegratedSystem linkedSystem)
            : base(name, ChannelTypes.Web, linkedSystem)
        {
        }

        public string Endpoint { get; set; }
        public override IDictionary<string, string> Properties =>
        new Dictionary<string, string>
        {
            {"endpoint", Endpoint}
        };
#nullable enable
        public string? ApplicationSecret => (this.LinkedIntegratedSystem as WebsiteIntegratedSystem)?.ApplicationSecret;
    }
}

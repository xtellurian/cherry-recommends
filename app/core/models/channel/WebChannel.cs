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
        public bool PopupAskForEmail { get; set; }
        public override IDictionary<string, object> Properties =>
        new Dictionary<string, object>
        {
            {"endpoint", Endpoint},
            {"popupAskForEmail", PopupAskForEmail}
        };
#nullable enable
        public string? ApplicationSecret => (this.LinkedIntegratedSystem as WebsiteIntegratedSystem)?.ApplicationSecret;
    }
}

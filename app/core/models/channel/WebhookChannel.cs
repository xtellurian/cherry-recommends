﻿namespace SignalBox.Core
{

    public class WebhookChannel : ChannelBase
    {
        protected WebhookChannel()
        { }

        public WebhookChannel(string name, IntegratedSystem linkedSystem)
            : base(name, ChannelTypes.Webhook, linkedSystem)
        {
        }

        public string Endpoint { get; set; }

    }
}

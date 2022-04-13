using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class ChannelWorkflow : IWorkflow, IChannelWorkflow
    {
        private readonly IWebhookChannelStore webhookChannelStore;
        private readonly IWebChannelStore webChannelStore;
        private readonly ILogger<ChannelWorkflow> logger;

        public ChannelWorkflow(IWebhookChannelStore webhookChannelStore,
                               IWebChannelStore webChannelStore,
                               ILogger<ChannelWorkflow> logger)
        {
            this.webChannelStore = webChannelStore;
            this.webhookChannelStore = webhookChannelStore;
            this.logger = logger;
        }

        public async Task<ChannelBase> CreateChannel(string name, ChannelTypes type, IntegratedSystem integratedSystem)
        {
            switch (type)
            {
                case ChannelTypes.Webhook:
                    return await CreateWebhookChannel(name, integratedSystem);
                case ChannelTypes.Web:
                    return await CreateWebChannel(name, integratedSystem);
                default:
                    throw new BadRequestException($"Channel type {type} not supported");
            }
        }

        public async Task<ChannelBase> UpdateChannelEndpoint(ChannelBase channel, string endpoint)
        {
            return await UpdateChannelProperties(channel, endpoint);
        }

        private async Task<WebhookChannel> CreateWebhookChannel(string name, IntegratedSystem integratedSystem)
        {
            if (integratedSystem.SystemType != IntegratedSystemTypes.Custom)
            {
                throw new BadRequestException($"Webhook channel only supports Custom Integrated System");
            }

            var channel = await webhookChannelStore.Create(new WebhookChannel(name, integratedSystem));
            await webhookChannelStore.Context.SaveChanges();
            return channel;
        }

        private async Task<WebhookChannel> UpdateWebhookChannelEndpoint(WebhookChannel channel, string endpoint)
        {
            channel.Endpoint = endpoint;
            await webhookChannelStore.Context.SaveChanges();
            return channel;
        }

        public async Task<ChannelBase> UpdateChannelProperties(ChannelBase channel, string endpoint, bool? popupAskForEmail = null)
        {
            if (channel is WebhookChannel)
            {
                return await UpdateWebhookChannelEndpoint(channel as WebhookChannel, endpoint);
            }
            else if (channel is WebChannel)
            {
                return await UpdateWebChannelProperties(channel as WebChannel, endpoint, popupAskForEmail);
            }
            else
            {
                throw new BadRequestException($"Channel type {channel.ChannelType} not supported");
            }
        }

        private async Task<WebChannel> CreateWebChannel(string name, IntegratedSystem integratedSystem)
        {
            if (integratedSystem.SystemType != IntegratedSystemTypes.Website)
            {
                throw new BadRequestException($"Web channel only supports Website Integrated System");
            }

            var channel = await webChannelStore.Create(new WebChannel(name, integratedSystem));
            await webChannelStore.Context.SaveChanges();
            return channel;
        }

        private async Task<WebChannel> UpdateWebChannelProperties(WebChannel channel, string endpoint, bool? popupAskForEmail = null)
        {
            channel.Endpoint = endpoint;
            channel.PopupAskForEmail = popupAskForEmail ?? channel.PopupAskForEmail;
            await webChannelStore.Context.SaveChanges();
            return channel;
        }
    }
}
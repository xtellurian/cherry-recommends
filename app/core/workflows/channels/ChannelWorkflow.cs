using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class ChannelWorkflow : IWorkflow, IChannelWorkflow
    {

        private readonly IWebhookChannelStore webhookChannelStore;
        private readonly ILogger<ChannelWorkflow> logger;

        public ChannelWorkflow(IWebhookChannelStore webhookChannelStore,
                               ILogger<ChannelWorkflow> logger)
        {
            this.webhookChannelStore = webhookChannelStore;
            this.logger = logger;
        }

        public async Task<ChannelBase> CreateChannel(string name, ChannelTypes type, IntegratedSystem integratedSystem)
        {
            switch (type)
            {
                case ChannelTypes.Webhook:
                    return await CreateWebhookChannel(name, integratedSystem);
                default:
                    throw new BadRequestException($"Channel type {type} not supported");
            }
        }

        private async Task<WebhookChannel> CreateWebhookChannel(string name, IntegratedSystem integratedSystem)
        {
            var channel = await webhookChannelStore.Create(new WebhookChannel(name, integratedSystem));
            await webhookChannelStore.Context.SaveChanges();
            return channel;
        }
    }
}
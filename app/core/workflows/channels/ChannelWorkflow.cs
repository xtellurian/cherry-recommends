using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Workflows
{
    public class ChannelWorkflow : IWorkflow, IChannelWorkflow
    {
        private readonly IWebhookChannelStore webhookChannelStore;
        private readonly IEmailChannelStore emailChannelStore;
        private readonly IWebChannelStore webChannelStore;
        private readonly ILogger<ChannelWorkflow> logger;

        public ChannelWorkflow(IWebhookChannelStore webhookChannelStore,
                               IWebChannelStore webChannelStore,
                               IEmailChannelStore emailChannelStore,
                               ILogger<ChannelWorkflow> logger)
        {
            this.webChannelStore = webChannelStore;
            this.webhookChannelStore = webhookChannelStore;
            this.emailChannelStore = emailChannelStore;
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
                case ChannelTypes.Email:
                    return await CreateEmailChannel(name, integratedSystem);
                default:
                    throw new BadRequestException($"Channel type {type} not supported");
            }
        }

        private async Task<EmailChannel> CreateEmailChannel(string name, IntegratedSystem integratedSystem)
        {
            if (integratedSystem.SystemType != IntegratedSystemTypes.Klaviyo)
            {
                throw new BadRequestException($"Email channel only supports Klaviyo Integrated System");
            }

            var channel = await emailChannelStore.Create(new EmailChannel(name, integratedSystem));
            await emailChannelStore.Context.SaveChanges();
            return channel;
        }

        public async Task<ChannelBase> UpdateChannelEndpoint(ChannelBase channel, string endpoint)
        {
            if (channel is WebhookChannel webhookChannel)
            {
                return await UpdateWebhookChannelEndpoint(webhookChannel, endpoint);
            }
            else
            {
                throw new BadRequestException($"Channel type {channel.ChannelType} does not have endpoint");
            }
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

        public async Task<ChannelBase> UpdateWebChannelProperties(ChannelBase channel, string host, bool? popupAskForEmail = null, int? popupDelay = null, string popupHeader = "", string popupSubheader = "", long? recommenderId = null, string customerIdPrefix = "", string storageType = "")
        {
            if (channel is WebChannel webChannel)
            {
                return await UpdateWebChannelProperties(webChannel, host, popupAskForEmail, popupDelay, popupHeader, popupSubheader, recommenderId, customerIdPrefix, storageType);
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

        private async Task<WebChannel> UpdateWebChannelProperties(WebChannel channel, string host, bool? popupAskForEmail = null, int? popupDelay = null, string popupHeader = "", string popupSubheader = "", long? recommenderId = null, string customerIdPrefix = "", string storageType = "")
        {
            channel.Host = host;
            channel.PopupAskForEmail = popupAskForEmail ?? channel.PopupAskForEmail;
            channel.PopupDelay = popupDelay ?? channel.PopupDelay;
            channel.PopupHeader = popupHeader ?? channel.PopupHeader;
            channel.PopupSubheader = popupSubheader ?? channel.PopupSubheader;
            channel.RecommenderIdToInvoke = recommenderId;
            channel.CustomerIdPrefix = customerIdPrefix ?? channel.CustomerIdPrefix;
            channel.StorageType = storageType ?? channel.StorageType;
            await webChannelStore.Context.SaveChanges();
            return channel;
        }

        public async Task<ChannelBase> UpdateEmailChannelListTrigger(ChannelBase channel, string listId, string listName)
        {
            if (channel is EmailChannel emailChannel)
            {
                return await UpdateEmailChannelListTrigger(emailChannel, listId, listName);
            }
            else
            {
                throw new BadRequestException($"Channel type {channel.ChannelType} not supported");
            }
        }

        private async Task<EmailChannel> UpdateEmailChannelListTrigger(EmailChannel channel, string listId, string listName)
        {
            channel.ListTriggerId = listId;
            channel.ListTriggerName = listName;
            await webhookChannelStore.Context.SaveChanges();
            return channel;
        }

        public Task<bool> CanSend(ChannelBase channel, RecommendationEntity recommendation)
        {
            var canSend = false;
            if (channel is EmailChannel)
            {
                canSend = !string.IsNullOrEmpty(recommendation.Customer.Email);
            }
            logger.LogInformation($"Can send returned {canSend} for recommendation {recommendation.Id} to channel {channel.Id}.");

            return Task.FromResult(canSend);
        }
    }
}
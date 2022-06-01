using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Workflows
{
    public class ChannelDeliveryWorkflow : IWorkflow, IChannelDeliveryWorkflow
    {
        private readonly ILogger<ChannelDeliveryWorkflow> logger;
        private readonly IChannelStore channelStore;
        private readonly IDeferredDeliveryStore deferredDeliveryStore;
        private readonly IChannelWorkflow channelWorkflow;
        private readonly IKlaviyoSystemWorkflow klaviyoWorkflow;

        public ChannelDeliveryWorkflow(ILogger<ChannelDeliveryWorkflow> logger,
                                        IStoreCollection storeCollection,
                                        IChannelWorkflow channelWorkflow,
                                        IKlaviyoSystemWorkflow klaviyoWorkflow)
        {
            this.channelStore = storeCollection.ResolveStore<IChannelStore, ChannelBase>();
            this.deferredDeliveryStore = storeCollection.ResolveStore<IDeferredDeliveryStore, DeferredDelivery>();
            this.channelWorkflow = channelWorkflow;
            this.klaviyoWorkflow = klaviyoWorkflow;
            this.logger = logger;
        }

        public async Task SendToChannel(ChannelBase channel, RecommendationEntity recommendation)
        {
            await SendRecommendationToChannel(channel, recommendation);
        }

        private async Task<bool> SendRecommendationToChannel(ChannelBase channel, RecommendationEntity recommendation, bool createDeferredIfCannotSend = true)
        {
            var canSend = await channelWorkflow.CanSend(channel, recommendation);
            if (canSend)
            {
                if (channel is EmailChannel emailChannel)
                {
                    await channelStore.Load(channel, _ => _.LinkedIntegratedSystem);
                    if (emailChannel.LinkedIntegratedSystem.SystemType == IntegratedSystemTypes.Klaviyo)
                    {
                        await klaviyoWorkflow.SendRecommendation(emailChannel, recommendation);
                        return true;
                    }
                    else
                    {
                        logger.LogWarning($"WARN: Unable to send email to channel {channel.Id}");
                    }
                }
            }
            else if (createDeferredIfCannotSend)
            {
                // create deferred delivery and add to database
                var deferredDelivery = new DeferredDelivery(channel, recommendation as ItemsRecommendation);
                await deferredDeliveryStore.Create(deferredDelivery);
                await deferredDeliveryStore.Context.SaveChanges();
            }

            return false;
        }

        public async Task OnCustomerUpdated(long customerId)
        {
            // get deferred delivery 
            var deliveries = await deferredDeliveryStore.QueryForCustomer(customerId);
            foreach (var delivery in deliveries)
            {
                var result = await SendRecommendationToChannel(delivery.Channel, delivery.Recommendation, false);
                if (result)
                {
                    await deferredDeliveryStore.Remove(delivery.Id);
                    await deferredDeliveryStore.Context.SaveChanges();
                }
            }
        }
    }
}
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
        private readonly IDateTimeProvider dateTimeProvider;

        public ChannelDeliveryWorkflow(ILogger<ChannelDeliveryWorkflow> logger,
                                        IDateTimeProvider dateTimeProvider,
                                        IStoreCollection storeCollection,
                                        IChannelWorkflow channelWorkflow,
                                        IKlaviyoSystemWorkflow klaviyoWorkflow)
        {
            this.channelStore = storeCollection.ResolveStore<IChannelStore, ChannelBase>();
            this.deferredDeliveryStore = storeCollection.ResolveStore<IDeferredDeliveryStore, DeferredDelivery>();
            this.channelWorkflow = channelWorkflow;
            this.klaviyoWorkflow = klaviyoWorkflow;
            this.dateTimeProvider = dateTimeProvider;
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
                // check if we can send it
                if (delivery.Sending != true && await channelWorkflow.CanSend(delivery.Channel, delivery.Recommendation))
                {
                    delivery.Sending = true;
                    await deferredDeliveryStore.Context.SaveChanges();
                    // mark this as sending
                    try
                    {
                        var result = await SendRecommendationToChannel(delivery.Channel, delivery.Recommendation, false);
                        if (result)
                        {
                            await deferredDeliveryStore.Remove(delivery.Id);
                        }
                        else
                        {
                            delivery.Sending = false;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logger.LogError(ex.Message);
                        throw new WorkflowException("Error sending to channgel", ex);
                    }
                    finally
                    {
                        await deferredDeliveryStore.Context.SaveChanges();
                    }
                }
            }
        }
    }
}

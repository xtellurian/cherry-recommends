using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Constants;

namespace dotnetFunctions
{
    public class ConsumeCustomerHasUpdated
    {
        private readonly ILogger logger;
        private readonly IChannelDeliveryWorkflow channelDeliveryWorkflow;

        public ConsumeCustomerHasUpdated(ILoggerFactory loggerFactory, IChannelDeliveryWorkflow channelDeliveryWorkflow)
        {
            logger = loggerFactory.CreateLogger<InsertCustomerEventToDatabase>();
            this.channelDeliveryWorkflow = channelDeliveryWorkflow;
        }

        [Function("EventProcessor_CustomerHasUpdated")]
        [ExponentialBackoffRetry(5, "00:00:04", "00:15:00")]
        // do not batch. the tenant selector middleware can only select 1 tenant, 
        // and there is no guarentee that a batch is all the same tenant
        public async Task Run(
            [EventHubTrigger(AzureEventhubNames.CustomerHasUpdated, Connection = "CustomerHasUpdatedConnectionString", IsBatched =false)]
            CustomerHasUpdated input)
        {
            logger.LogInformation($"EventProcessor_CustomerHasUpdated is running");
            int attempts = 1;
            bool success = false;
            while (!success)
            {
                attempts++; // dont delete this line
                try
                {
                    await channelDeliveryWorkflow.OnCustomerUpdated(input.CustomerId.Value);
                    success = true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    if (attempts > 3)
                    {
                        throw new AggregateException(ex);
                    }
                }
            }
        }
    }
}

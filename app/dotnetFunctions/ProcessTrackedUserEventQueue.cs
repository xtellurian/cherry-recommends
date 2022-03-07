using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class ProcessTrackedUserEventQueue
    {
        private readonly CustomerEventsWorkflows eventWorkflow;
        private readonly CustomerWorkflows customerWorkflows;
        private readonly IQueueMessagesFileStore queueMessagesFileStore;

        public ProcessTrackedUserEventQueue(CustomerEventsWorkflows eventWorkflow,
                                            CustomerWorkflows customerWorkflows,
                                            IQueueMessagesFileStore queueMessagesFileStore)
        {
            this.eventWorkflow = eventWorkflow;
            this.customerWorkflows = customerWorkflows;
            this.queueMessagesFileStore = queueMessagesFileStore;
        }

        [Function("ProcessTrackedUserEventQueue")]
        public async Task ProcessTrackedUserEvents(
            [QueueTrigger(SignalBox.Core.Constants.AzureQueueNames.TrackedUserEvents)] TrackedUserEventsQueueMessage message,
            FunctionContext context)
        {
            var logger = context.GetLogger(nameof(ProcessTrackedUserEvents));

            logger.LogInformation($"Processing message with path: {message.Path}");

            // read the data then pass to workflow
            var content = await queueMessagesFileStore.ReadAsString(message.Path);
            logger.LogInformation($"Deserialised file body.");
            var events = JsonSerializer.Deserialize<List<CustomerEventInput>>(content, eventWorkflow.SerializerOptions);

            try
            {
                await eventWorkflow.ProcessEvents(events);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw new System.Exception(ex.Message, ex);
            }

        }

        [Function("ProcessNewTrackedUsersEventQueue")]
        public async Task ProcessNewTrackedUsersEvents(
            [QueueTrigger(Core.Constants.AzureQueueNames.NewTrackedUsers)] NewCustomerEventQueueMessage queueMessage,
            FunctionContext context)
        {
            var logger = context.GetLogger(nameof(ProcessNewTrackedUsersEvents));

            logger.LogInformation($"Processing message with {queueMessage.PendingCustomers.Count()} create customer messages: ");
            try
            {
                await customerWorkflows.CreateOrUpdate(queueMessage.PendingCustomers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

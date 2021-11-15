using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using static SignalBox.Core.Workflows.TrackedUserEventsWorkflows;

namespace SignalBox.Functions
{
    public class ProcessTrackedUserEventQueue
    {
        private readonly TrackedUserEventsWorkflows eventWorkflow;
        private readonly TrackedUserWorkflows trackedUserWorkflows;
        private readonly IQueueMessagesFileStore queueMessagesFileStore;

        public ProcessTrackedUserEventQueue(TrackedUserEventsWorkflows eventWorkflow,
                                            TrackedUserWorkflows trackedUserWorkflows,
                                            IQueueMessagesFileStore queueMessagesFileStore)
        {
            this.eventWorkflow = eventWorkflow;
            this.trackedUserWorkflows = trackedUserWorkflows;
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
            var events = JsonSerializer.Deserialize<List<TrackedUserEventInput>>(content, eventWorkflow.SerializerOptions);

            try
            {
                await eventWorkflow.TrackUserEvents(events, addToQueue: false);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw new System.Exception(ex.Message, ex);
            }

        }

        [Function("ProcessNewTrackedUsersEventQueue")]
        public async Task ProcessNewTrackedUsersEvents(
            [QueueTrigger(SignalBox.Core.Constants.AzureQueueNames.NewTrackedUsers)] NewTrackedUserEventQueueMessage message,
            FunctionContext context)
        {
            var logger = context.GetLogger(nameof(ProcessNewTrackedUsersEvents));

            logger.LogInformation($"Processing message with {message.CommonIds.Count()} commonUserIds: ");
            try
            {
                await trackedUserWorkflows.CreateIfNotExist(message.CommonIds);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw new System.Exception(ex.Message, ex);
            }
        }
    }
}

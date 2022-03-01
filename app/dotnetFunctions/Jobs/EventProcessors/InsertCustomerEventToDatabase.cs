using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Constants;
using SignalBox.Core.Workflows;
using static SignalBox.Core.Workflows.CustomerEventsWorkflows;

namespace dotnetFunctions
{
    public class InsertCustomerEventToDatabase
    {
        private readonly ILogger logger;
        private readonly CustomerEventsWorkflows eventsWorkflows;

        public InsertCustomerEventToDatabase(ILoggerFactory loggerFactory, CustomerEventsWorkflows eventsWorkflows)
        {
            logger = loggerFactory.CreateLogger<InsertCustomerEventToDatabase>();
            this.eventsWorkflows = eventsWorkflows;
        }

        [Function("EventProcessor_InsertToDb")]
        [ExponentialBackoffRetry(5, "00:00:04", "00:15:00")]
        // do not batch. the tenant selector middleware can only select 1 tenant, 
        // and there is no guarentee that a batch is all the same tenant
        public async Task Run(
            [EventHubTrigger(AzureEventhubNames.EventIngestion, Connection = "EventIngestionConnectionString", IsBatched =false)]
            CustomerEventInput input)
        {
            logger.LogInformation($"EventProcessor_InsertToDb is running");
            int attempts = 1;
            bool success = false;
            while (!success)
            {
                attempts++; // dont delete this line
                try
                {
                    var processResult = await eventsWorkflows.ProcessEvents(new List<CustomerEventInput> { input });
                    success = true;
                    logger.LogInformation($"EventProcessor_InsertToDb enqueued {processResult.EventsEnqueued}"
                        + $" and processed {processResult.EventsProcessed}");
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

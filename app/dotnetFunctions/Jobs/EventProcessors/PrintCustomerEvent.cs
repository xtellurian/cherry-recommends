using System;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Constants;

namespace dotnetFunctions
{
    public class PrintCustomerEvent
    {
        private readonly ILogger _logger;

        public PrintCustomerEvent(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PrintCustomerEvent>();
        }

        // event hub name is ignored ?? todo: verify that's true?
        [Function("EventProcessor_PrintCustomerEvent")]
        public void Run(
            [EventHubTrigger(AzureEventhubNames.EventIngestion,
                Connection = "EventIngestionConnectionString",
                ConsumerGroup = AzureEventhubConsumerGroups.Monitoring)]
            string[] input)
        {
            _logger.LogInformation($"Received {input.Length} messages.");
            foreach (var s in input)
            {

                _logger.LogInformation($"Message: {s}");
            }
        }
    }
}

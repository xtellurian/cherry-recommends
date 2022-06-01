using System;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Constants;

namespace dotnetFunctions
{
    public class PrintCustomerHasUpdated
    {
        private readonly ILogger _logger;

        public PrintCustomerHasUpdated(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PrintCustomerHasUpdated>();
        }

        [Function("EventProcessor_PrintCustomerHasUpdated")]
        public void Run(
            [EventHubTrigger(AzureEventhubNames.CustomerHasUpdated,
                Connection = "CustomerHasUpdatedConnectionString",
                ConsumerGroup = AzureEventhubConsumerGroups.Monitoring)]
            SignalBox.Core.CustomerHasUpdated[] input)
        {
            _logger.LogInformation($"Received {input.Length} messages.");
            foreach (var s in input)
            {
                _logger.LogInformation($"Customer updated id = {s.CustomerId} with event id = {s.EventId}");
            }
        }
    }
}

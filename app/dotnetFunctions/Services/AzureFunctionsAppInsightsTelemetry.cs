using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SignalBox.Core;

namespace SignalBox.Functions.Services
{
    public class AzureFunctionsAppInsightsTelemetry : ITelemetry
    {
        private readonly ILogger<AzureFunctionsAppInsightsTelemetry> logger;

        public AzureFunctionsAppInsightsTelemetry(ILogger<AzureFunctionsAppInsightsTelemetry> logger)
        {
            this.logger = logger;
        }

        public void TrackException(Exception exception)
        {
            logger.LogError("An exception was tracked in Azure Functions", exception);
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            logger.LogInformation($"Metric {name} = {value}", properties);
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            logger.LogInformation($"Event {name} occurred", properties);
        }
    }
}
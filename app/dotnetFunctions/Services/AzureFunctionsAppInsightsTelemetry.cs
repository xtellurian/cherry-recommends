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
            logger.LogInformation($"Metric: {name} = {value}", properties);
        }

        public void TrackTimingMetric(string name, TimeSpan value, string description)
        {
            TrackMetric(name, value.TotalMilliseconds, new Dictionary<string, string> { { "description", description } });
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            logger.LogInformation($"Event: {name} occurred", properties);
        }

        public System.Diagnostics.Stopwatch NewStopwatch(bool? start = null)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            if (start == true)
            {
                stopwatch.Start();
            }
            return stopwatch;
        }
        public void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            logger.LogInformation($"Dependency: {dependencyTypeName} {dependencyName} {data} took {duration.TotalSeconds} seconds to complete with success {success}");
        }
    }
}
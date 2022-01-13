using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using SignalBox.Core;

namespace SignalBox.Web.Services
{
    public class AppInsightsTelemetry : ITelemetry
    {
        private readonly TelemetryClient client;

        public AppInsightsTelemetry(TelemetryClient client)
        {
            this.client = client;
        }

        public void TrackException(Exception exception)
        {
            client.TrackException(exception);
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            client.TrackMetric(name, value, properties);
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            client.TrackEvent(name, properties);
        }

        public Stopwatch NewStopwatch(bool? start = null)
        {
            var stopwatch = new Stopwatch();
            if (start == true)
            {
                stopwatch.Start();
            }
            return stopwatch;
        }

        public void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            client.TrackDependency(dependencyTypeName, dependencyName, data, startTime, duration, success);
        }
    }
}
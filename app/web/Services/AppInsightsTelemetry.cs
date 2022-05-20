using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.ApplicationInsights;
using SignalBox.Core;

namespace SignalBox.Web.Services
{
    public class AppInsightsTelemetry : ITelemetry
    {
        private readonly TelemetryClient client;
        private readonly IServerTiming serverTiming;

        public AppInsightsTelemetry(TelemetryClient client, IServerTiming serverTiming)
        {
            this.client = client;
            this.serverTiming = serverTiming;
        }

        public void TrackException(Exception exception)
        {
            client.TrackException(exception);
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            client.TrackMetric(name, value, properties);
        }

        public void TrackTimingMetric(string name, TimeSpan value, string description)
        {
            serverTiming.Metrics.Add(new ServerTimingMetric(name, (decimal)value.TotalMilliseconds, description));
            // also track metric in the regular way
            TrackMetric(name, value.TotalMilliseconds, new Dictionary<string, string> { { "description", description } });
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            client.TrackEvent(name, properties);
        }

        public Stopwatch NewStopwatch(bool? start = true)
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
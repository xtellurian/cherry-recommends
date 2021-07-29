using System;
using System.Collections.Generic;
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
    }
}
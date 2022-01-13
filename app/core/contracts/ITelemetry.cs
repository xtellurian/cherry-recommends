using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SignalBox.Core
{
    public interface ITelemetry
    {
        void TrackMetric(string name, double value, IDictionary<string, string> properties = null);
        void TrackException(System.Exception exception);
        void TrackEvent(string name, IDictionary<string, string> properties = null);
        Stopwatch NewStopwatch(bool? start = null);
        void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success);
    }
}
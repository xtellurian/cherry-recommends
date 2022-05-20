using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SignalBox.Core
{
    public interface ITelemetry
    {
        void TrackMetric(string name, double value, IDictionary<string, string> properties = null);
        /// <summary>
        /// Adds a timing metric, available in the response header.
        /// DO NOT INCLUDE SENSITIVE INFORMATION
        /// </summary>
        /// <param name="name">Name of the operation </param>
        /// <param name="value">Timespan of the time taken to do an operation</param>
        /// <param name="description">Describe the operation</param>
        void TrackTimingMetric(string name, TimeSpan value, string description);
        void TrackException(Exception exception);
        void TrackEvent(string name, IDictionary<string, string> properties = null);
        Stopwatch NewStopwatch(bool? start = true);
        void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success);
    }
}
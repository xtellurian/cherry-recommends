using System.Collections.Generic;

namespace SignalBox.Core
{
    public interface ITelemetry
    {
        void TrackMetric(string name, double value, IDictionary<string, string> properties = null);
        void TrackException(System.Exception exception);
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class EventKindSummary
    {
        public EventKindSummary(Dictionary<string, EventStats> eventTypeInstanceCounts)
        {
            EventTypes = eventTypeInstanceCounts;
            InstanceCount = eventTypeInstanceCounts.Values.Select(_ => _.Instances).Sum();
        }

        public IEnumerable<string> Keys => EventTypes.Keys;
        public int InstanceCount { get; set; }
        public Dictionary<string, EventStats> EventTypes { get; set; }
    }
}
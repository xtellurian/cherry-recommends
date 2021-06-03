using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class TrackedUserEventSummary
    {
        public TrackedUserEventSummary()
        {
            Kinds = new Dictionary<string, EventKindSummary>();
        }

        public IEnumerable<string> Keys => Kinds.Keys;

        public Dictionary<string, EventKindSummary> Kinds { get; set; }

        public void AddKind(string kind, Dictionary<string, EventStats> eventTypeCounts)
        {
            Kinds.Add(kind, new EventKindSummary(eventTypeCounts));
        }
    }
}
using System.Collections.Generic;

namespace SignalBox.Core
{
    public class CustomerEventSummary
    {
        public CustomerEventSummary()
        {
            Kinds = new Dictionary<EventKinds, EventKindSummary>();
        }

        public IEnumerable<EventKinds> Keys => Kinds.Keys;

        public Dictionary<EventKinds, EventKindSummary> Kinds { get; set; }

        public void AddKind(EventKinds kind, Dictionary<string, EventStats> eventTypeCounts)
        {
            Kinds.Add(kind, new EventKindSummary(eventTypeCounts));
        }
        public void Add(EventKinds kind, EventKindSummary s)
        {
            Kinds.Add(kind, s);
        }
    }
}
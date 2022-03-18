using System.Collections.Generic;

namespace SignalBox.Core
{
    public class CustomerEventKindSummary
    {
        public CustomerEventKindSummary(EventKinds kind, EventKindSummary summary)
        {
            this.Kind = kind;
            this.Summary = summary;
        }

        public EventKinds Kind { get; set; }
        public EventKindSummary Summary { get; set; }
    }
}
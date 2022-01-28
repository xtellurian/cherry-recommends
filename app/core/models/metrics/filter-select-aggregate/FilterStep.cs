namespace SignalBox.Core.Metrics
{
#nullable enable
    public class FilterStep
    {
        protected FilterStep()
        { }

        public FilterStep(string? eventTypeMatch)
        {
            this.EventTypeMatch = eventTypeMatch;
        }

        // match only these event types.
        // null matches all event types
        public string? EventTypeMatch { get; set; }
    }
}
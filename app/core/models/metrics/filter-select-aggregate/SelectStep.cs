namespace SignalBox.Core.Metrics
{
#nullable enable

    public class SelectStep
    {
        protected SelectStep()
        { }

        public SelectStep(string? propertynameMatch)
        {
            this.PropertyNameMatch = propertynameMatch;
        }

        // select the property with this name
        // if null, then just count every event.
        public string? PropertyNameMatch { get; set; }
    }
}
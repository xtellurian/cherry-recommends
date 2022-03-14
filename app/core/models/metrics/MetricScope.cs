namespace SignalBox.Core.Metrics
{
    /// <summary>
    /// Options for the scope of a Metric. 
    /// The scope indicates whether a metric applies 
    /// - to a single customer (e.g. average # of purchases last week)
    /// - to a business (e.g. average # of purchases of customers belonging to a business last week)
    /// - globally (e.g. conversion rate)
    /// </summary>
    public enum MetricScopes
    {
        Customer, // defaults to customer, it's the first
        Business,
        Global,
    }
}
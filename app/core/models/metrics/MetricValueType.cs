namespace SignalBox.Core.Metrics
{
    /// <summary>
    /// Either Numeric for number values (e.g. total revenue), or Categorial for classes of value, e.g. Champion/ Flakey
    /// </summary>
    public enum MetricValueType
    {
        Numeric,
        Categorical
    }
}
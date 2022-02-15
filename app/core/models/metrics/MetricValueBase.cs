using System.Text.Json.Serialization;

namespace SignalBox.Core.Metrics
{
#nullable enable
    public abstract class MetricValueBase : Entity, IHierarchyBase
    {
        protected MetricValueBase()
        {
            Metric = null!;
        }
        private MetricValueBase(Metric metric, int version)
        {
            Metric = metric;
            Version = version;
        }
        protected MetricValueBase(Metric metric, int version, string stringValue) : this(metric, version)
        {
            StringValue = stringValue;
        }
        protected MetricValueBase(Metric metric, int version, double numericValue) : this(metric, version)
        {
            NumericValue = numericValue;
        }

        public long MetricId { get; set; }
        public Metric Feature => Metric;
        public Metric Metric { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? NumericValue { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StringValue { get; set; }
        public object? Value => (object?)NumericValue ?? (object?)StringValue;
        public int Version { get; set; }
        public string? Discriminator { get; set; }

        public bool IsNumeric()
        {
            return NumericValue != null;
        }

        public bool ValuesEqual(MetricValueBase other)
        {
            if (this.Value == null || other?.Value == null)
            {
                // nulls cannot be equal
                return false;
            }
            else
            {
                return string.Equals(this.Value.ToString(), other.Value.ToString());
            }
        }
    }
}
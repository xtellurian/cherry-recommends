using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class CustomerMetricBase : Entity
    {
        protected CustomerMetricBase()
        { }
#nullable enable
        private CustomerMetricBase(Customer customer, Metric metric)
        {
            Customer = customer;
            Metric = metric;
        }
        public CustomerMetricBase(Customer customer, Metric metric, string value)
        : this(customer, metric)
        {
            StringValue = value;
        }

        public CustomerMetricBase(Customer customer, Metric metric, double value)
        : this(customer, metric)
        {
            NumericValue = value;
        }

        public bool ValuesEqual(CustomerMetricBase other)
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

        public bool IsNumeric()
        {
            return this.NumericValue != null;
        }


        public long TrackedUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer TrackedUser => Customer;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer Customer { get; set; }
        public long MetricId { get; set; }
        public Metric Feature => Metric;
        public Metric Metric { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? NumericValue { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StringValue { get; set; }
        public object? Value => (object?)NumericValue ?? (object?)StringValue;
    }
}
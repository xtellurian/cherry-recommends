using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUserFeatureBase : Entity
    {
        protected TrackedUserFeatureBase()
        { }
#nullable enable
        private TrackedUserFeatureBase(Customer customer, Feature feature)
        {
            Customer = customer;
            Feature = feature;
        }
        public TrackedUserFeatureBase(Customer customer, Feature feature, string value)
        : this(customer, feature)
        {
            StringValue = value;
        }

        public TrackedUserFeatureBase(Customer customer, Feature feature, double value)
        : this(customer, feature)
        {
            NumericValue = value;
        }

        public bool ValuesEqual(TrackedUserFeatureBase other)
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
        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? NumericValue { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StringValue { get; set; }
        public object? Value => (object?)NumericValue ?? (object?)StringValue;
    }
}
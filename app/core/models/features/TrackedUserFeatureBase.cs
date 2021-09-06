using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUserFeatureBase : Entity
    {
        protected TrackedUserFeatureBase()
        { }
#nullable enable
        protected TrackedUserFeatureBase(TrackedUser trackedUser, Feature feature)
        {
            TrackedUser = trackedUser;
            Feature = feature;
        }
        public TrackedUserFeatureBase(TrackedUser trackedUser, Feature feature, string value)
        : this(trackedUser, feature)
        {
            StringValue = value;
        }

        public TrackedUserFeatureBase(TrackedUser trackedUser, Feature feature, double value)
        : this(trackedUser, feature)
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
        public TrackedUser TrackedUser { get; set; }
        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? NumericValue { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StringValue { get; set; }
        public object? Value => (object?)NumericValue ?? (object?)StringValue;
    }
}
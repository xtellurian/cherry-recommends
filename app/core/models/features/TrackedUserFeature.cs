using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUserFeature : Entity
    {
        protected TrackedUserFeature()
        { }
        private TrackedUserFeature(TrackedUser trackedUser, Feature feature, int version)
        {
            TrackedUser = trackedUser;
            Feature = feature;
            Version = version;
        }
        public TrackedUserFeature(TrackedUser trackedUser, Feature feature, string value, int version)
        : this(trackedUser, feature, version)
        {
            StringValue = value;
        }

        public TrackedUserFeature(TrackedUser trackedUser, Feature feature, double value, int version)
        : this(trackedUser, feature, version)
        {
            NumericValue = value;
        }

        public bool ValuesEqual(TrackedUserFeature other)
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

#nullable enable
        public int Version { get; set; }
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
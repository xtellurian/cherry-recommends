using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUser : CommonEntity
    {
        protected TrackedUser()
        { }

        public TrackedUser(string commonUserId) : base(commonUserId, null)
        { }

        public TrackedUser(string commonUserId, string name) : base(commonUserId, name)
        { }

        public TrackedUser(string commonUserId, string name, DynamicPropertyDictionary properties) : this(commonUserId, name)
        {
            if (properties != null)
            {
                this.Properties = properties;
            }
        }

        public TrackedUser(string commonUserId, string name, IDictionary<string, object> properties)
        : this(commonUserId, name, new DynamicPropertyDictionary(properties))
        { }

        public string CommonUserId => CommonId;

        [JsonIgnore]
        public ICollection<TrackedUserAction> Actions { get; set; }
        [JsonIgnore]
        public ICollection<Segment> Segments { get; set; }

        [JsonIgnore]
        public ICollection<TrackedUserTouchpoint> TrackedUserTouchpoints { get; set; }
        [JsonIgnore]
        public ICollection<HistoricTrackedUserFeature> HistoricTrackedUserFeatures { get; set; }
        // this can be serialised and sent out
        public ICollection<TrackedUserSystemMap> IntegratedSystemMaps { get; set; } = new List<TrackedUserSystemMap>(); // for initialising
    }
}

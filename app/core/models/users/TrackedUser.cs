using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUser : NamedEntity
    {
        public TrackedUser()
        { }

        public TrackedUser(string commonUserId)
        {
            CommonUserId = commonUserId;
        }

        public TrackedUser(string commonUserId, string name) : base(name)
        {
            CommonUserId = commonUserId;
        }

        public TrackedUser(string commonUserId, string name, DynamicPropertyDictionary properties) : this(commonUserId, name)
        {
            if (properties != null)
            {
                this.Properties = properties;
            }
        }

        public string CommonUserId { get; set; }
        public DynamicPropertyDictionary Properties { get; set; } = new DynamicPropertyDictionary(); // not empty

        [JsonIgnore]
        public ICollection<Segment> Segments { get; set; }
    }
}

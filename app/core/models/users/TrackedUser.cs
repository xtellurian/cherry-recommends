using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class TrackedUser : NamedEntity
    {
        public TrackedUser()
        { }

        public TrackedUser(string externalId)
        {
            ExternalId = externalId;
        }

        public TrackedUser(string externalId, string name) : base(name)
        {
            ExternalId = externalId;
        }

        public string ExternalId { get; set; }

        [JsonIgnore]
        public ICollection<Segment> Segments { get; set; }
    }
}

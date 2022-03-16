using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Segment : EnvironmentScopedEntity
    {
        protected Segment()
        { }
#nullable enable
        public Segment(string name)
        {
            Name = name;
        }

        [JsonIgnore]
        public ICollection<CustomerSegment> InSegment { get; set; } = new Collection<CustomerSegment>();
        [JsonIgnore]
        public ICollection<RecommenderSegment> RecommenderSegments { get; set; } = new Collection<RecommenderSegment>();

        public string? Name { get; set; }

        public static Segment MoreThan10Events => new Segment
        {
            Id = 100,
            Name = "More than 10 Events"
        };
    }
}
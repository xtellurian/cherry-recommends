using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SignalBox.Core
{
    public class Segment : EnvironmentScopedEntity
    {
        public Segment()
        { }

        public Segment(string name) : base(name)
        { }

        public ICollection<TrackedUser> InSegment { get; set; } = new Collection<TrackedUser>();
    }
}
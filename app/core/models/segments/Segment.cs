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

        public ICollection<Customer> InSegment { get; set; } = new Collection<Customer>();
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public ICollection<Customer> InSegment { get; set; } = new Collection<Customer>();

        public string? Name { get; set; }
    }
}
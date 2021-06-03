using System;

namespace SignalBox.Core
{
    public abstract class Entity
    {
        protected Entity()
        { }

        public long Id { get; set; } // set when created
        // when in database, format string is: %Y-%m-%dT%H:%M:%S.%f%z
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
    }
}
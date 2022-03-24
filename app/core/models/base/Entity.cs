using System;

namespace SignalBox.Core
{
    /// <summary>
    /// The base class for all independent entites that exist in the database.
    /// Owned entities should use OwnedEntity.
    /// </summary>
    public abstract class Entity
    {
        protected Entity()
        { }

        public virtual void Validate() { }

        /// <summary>
        /// The primary key
        /// </summary>
        public long Id { get; set; } // set when created
        // when in database, format string is: %Y-%m-%dT%H:%M:%S.%f%z
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        // useful conversion methods
        protected string Serialize<T>(T value)
        {
            return Serialization.Serializer.Serialize(value);
        }

        public T Deserialize<T>(string value) where T : class
        {
            return Serialization.Serializer.Deserialize<T>(value);
        }
    }
}
using System;
using System.Text.Json;

namespace SignalBox.Core
{
    public abstract class Entity
    {
        private JsonSerializerOptions serializerOptions => new JsonSerializerOptions();
        protected Entity()
        { }

        public virtual void Validate() { }

        public long Id { get; set; } // set when created
        // when in database, format string is: %Y-%m-%dT%H:%M:%S.%f%z
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        // useful conversion methods
        protected string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, typeof(T), serializerOptions);
        }

        public T Deserialize<T>(string value)
        {
            if (value == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(value, serializerOptions);
        }

    }
}
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
#nullable enable
    public abstract class EnvironmentScopedEntity : Entity
    {
        protected EnvironmentScopedEntity()
        { }

        protected EnvironmentScopedEntity(string? name)
        {
            Name = name;
        }
        public string? Name { get; set; } // settable by the creator
        public long? EnvironmentId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Environment? Environment { get; set; }
    }
}
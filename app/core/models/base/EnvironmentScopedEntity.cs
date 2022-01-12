using System.Text.Json.Serialization;

namespace SignalBox.Core
{
#nullable enable
    public abstract class EnvironmentScopedEntity : Entity
    {
        protected EnvironmentScopedEntity()
        { }

        public long? EnvironmentId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Environment? Environment { get; set; }
    }
}
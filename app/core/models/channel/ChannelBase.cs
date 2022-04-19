using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public abstract class ChannelBase : EnvironmentScopedEntity, IHierarchyBase
    {
        protected ChannelBase()
        { }

        public ChannelBase(string name, ChannelTypes type, IntegratedSystem linkedSystem)
        {
            Name = name;
            ChannelType = type;
            LinkedIntegratedSystem = linkedSystem;
        }


        public string Name { get; set; }
        public ChannelTypes ChannelType { get; set; }
        public long LinkedIntegratedSystemId { get; set; }
        public IntegratedSystem LinkedIntegratedSystem { get; set; }

#nullable enable
        public string? Discriminator { get; set; }
        public DateTimeOffset? LastEnqueued { get; set; }
        public DateTimeOffset? LastCompleted { get; set; }
        public abstract IDictionary<string, object>? Properties { get; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<RecommenderEntityBase> Recommenders { get; set; } = new List<RecommenderEntityBase>();
    }
}

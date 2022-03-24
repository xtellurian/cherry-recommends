using System;

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
    }
}

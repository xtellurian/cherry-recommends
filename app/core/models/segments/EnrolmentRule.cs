using System;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Segments
{
#nullable enable
    public abstract class EnrolmentRule : Entity, IHierarchyBase, IBackgroundJob
    {
        public string Discriminator { get; set; } = null!;
        public long SegmentId { get; set; }
        [JsonIgnore]
        public Segment Segment { get; set; } = null!;
        public DateTimeOffset? LastEnqueued { get; set; }
        public DateTimeOffset? LastCompleted { get; set; }
    }
}
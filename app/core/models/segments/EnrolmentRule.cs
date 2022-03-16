namespace SignalBox.Core.Segments
{
#nullable enable
    public abstract class EnrolmentRule : Entity, IHierarchyBase
    {
        public string Discriminator { get; set; } = null!;
        public long SegmentId { get; set; }
        public Segment Segment { get; set; } = null!;
    }
}
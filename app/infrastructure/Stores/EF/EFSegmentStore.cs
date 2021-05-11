using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFSegmentStore : EFEntityStoreBase<Segment>, ISegmentStore
    {
        public EFSegmentStore(SignalBoxDbContext context)
        : base(context, (c) => c.Segments)
        {
        }
    }
}
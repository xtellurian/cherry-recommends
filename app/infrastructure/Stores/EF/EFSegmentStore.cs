using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFSegmentStore : EFEntityStoreBase<Segment>, ISegmentStore
    {
        public EFSegmentStore(SignalBoxDbContext context)
        : base(context, (c) => c.Segments)
        {
        }
    }
}
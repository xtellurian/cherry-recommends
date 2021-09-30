using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFSegmentStore : EFEntityStoreBase<Segment>, ISegmentStore
    {
        public EFSegmentStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.Segments)
        { }
    }
}
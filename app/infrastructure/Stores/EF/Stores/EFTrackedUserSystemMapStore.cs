using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserSystemMapStore : EFEntityStoreBase<TrackedUserSystemMap>, ITrackedUserSystemMapStore
    {
        public EFTrackedUserSystemMapStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.TrackUserSystemMaps)
        {
        }
    }
}
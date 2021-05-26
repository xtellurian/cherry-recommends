using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserSystemMapStore : EFEntityStoreBase<TrackedUserSystemMap>, ITrackedUserSystemMapStore
    {
        public EFTrackedUserSystemMapStore(SignalBoxDbContext context) : base(context, c => c.TrackUserSystemMaps)
        {
        }
    }
}
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public static class IntegratedSystemStoreExtensions
    {
        /// <summary>
        /// Looks up a tracked user using integrated system IDs.
        /// </summary>
        public static async Task<TrackedUser> ReadFromIntegratedSystem(this ITrackedUserSystemMapStore systemMapStore, long integratedSystemId, string externalSystemUserId)
        {
            var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.UserId == externalSystemUserId && _.IntegratedSystemId == integratedSystemId);
            if (systemMaps.Items.Count() > 0)
            {
                // this may cause issues if more than one, but for now, just return the first.
                var map = systemMaps.Items.First();
                await systemMapStore.Load(map, _ => _.TrackedUser);
                // happy path
                return map.TrackedUser;
            }
            else
            {
                throw new EntityNotFoundException(typeof(TrackedUserSystemMap), externalSystemUserId, "User Map not found");
            }
        }
        
     
        /// <summary>
        /// True if a user exists in the integrated system.
        /// </summary>
        public static async Task<bool> ExistsInIntegratedSystem(this ITrackedUserSystemMapStore systemMapStore, long integratedSystemId, string externalSystemUserId)
        {
            var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.UserId == externalSystemUserId && _.IntegratedSystemId == integratedSystemId);
            return systemMaps.Items.Any();
        }
    }
}
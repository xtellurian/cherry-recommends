using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public static class IntegratedSystemStoreExtensions
    {
        /// <summary>
        /// Looks up a tracked user using integrated system IDs.
        /// </summary>
        public static async Task<Customer> ReadFromIntegratedSystem(this ITrackedUserSystemMapStore systemMapStore, long integratedSystemId, string externalSystemUserId)
        {
            var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.UserId == externalSystemUserId && _.IntegratedSystemId == integratedSystemId);
            if (systemMaps.Items.Count() > 0)
            {
                // this may cause issues if more than one, but for now, just return the first.
                var map = systemMaps.Items.First();
                await systemMapStore.Load(map, _ => _.Customer);
                // happy path
                return map.Customer;
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

        /// <summary>
        /// Finds the map for a given system and tracked user
        /// </summary>
        public static async Task<TrackedUserSystemMap> FindMap(this ITrackedUserSystemMapStore systemMapStore, Customer customer, IntegratedSystem system)
        {
            var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.TrackedUserId == customer.Id && _.IntegratedSystemId == system.Id);
            if (systemMaps.Items.Any())
            {
                return systemMaps.Items.First();
            }
            else
            {
                throw new BadRequestException($"Tracked User {customer.Id} has no system map for system {system.Id}");
            }
        }

        /// <summary>
        /// Returns true if the map exists
        /// </summary>
        public static async Task<bool> MapExists(this ITrackedUserSystemMapStore systemMapStore, Customer customer, IntegratedSystem system)
        {
            var mapCount = await systemMapStore.Count(_ => _.TrackedUserId == customer.Id && _.IntegratedSystemId == system.Id);
            return mapCount > 0;

        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class TouchpointWorkflows
    {
        private readonly ITouchpointStore touchpointStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly ICustomerStore trackedUserStore;
        private readonly IStorageContext storageContext;

        public TouchpointWorkflows(ITouchpointStore touchpointStore,
                                   ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                   ICustomerStore trackedUserStore,
                                   IStorageContext storageContext)
        {
            this.touchpointStore = touchpointStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
            this.trackedUserStore = trackedUserStore;
            this.storageContext = storageContext;
        }

        public async Task<TrackedUserTouchpoint> CreateTouchpointOnUser(Customer customer,
                                                                   string touchpointCommonId,
                                                                   Dictionary<string, object> values)
        {
            Touchpoint touchpoint;
            if (await touchpointStore.ExistsFromCommonId(touchpointCommonId))
            {
                touchpoint = await touchpointStore.ReadFromCommonId(touchpointCommonId);
            }
            else
            {
                touchpoint = await touchpointStore.Create(new Touchpoint(touchpointCommonId, null));
            }

            var nextVersion = 1 + await trackedUserTouchpointStore.CurrentMaximumTouchpointVersion(customer, touchpoint);

            var tp = await trackedUserTouchpointStore.Create(new TrackedUserTouchpoint(customer, touchpoint, values, nextVersion));
            await storageContext.SaveChanges();
            return tp;
        }

        public async Task<Touchpoint> CreateTouchpoint(string commonId, string name)
        {
            var touchpoint = await touchpointStore.Create(new Touchpoint(commonId, name));
            await storageContext.SaveChanges();
            return touchpoint;
        }

        public async Task<Paginated<Customer>> GetTrackedUsers(Touchpoint touchpoint, int page)
        {
            return await touchpointStore.QueryTrackedUsers(page, touchpoint.Id);
        }

        public async Task<TrackedUserTouchpoint> ReadTouchpointValues(Customer customer, string touchpointCommonId, int? version = null)
        {
            var touchpoint = await touchpointStore.ReadFromCommonId(touchpointCommonId);
            return await trackedUserTouchpointStore.ReadTouchpoint(customer, touchpoint, version);
        }
    }
}
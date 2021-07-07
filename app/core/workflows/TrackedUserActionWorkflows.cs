using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class TrackedUserActionWorkflows : IWorkflow
    {
        private readonly ITrackedUserActionStore actionStore;
        private readonly IStorageContext storageContext;

        public TrackedUserActionWorkflows(ITrackedUserActionStore actionStore, IStorageContext storageContext)
        {
            this.actionStore = actionStore;
            this.storageContext = storageContext;
        }

        public async Task<IEnumerable<TrackedUserAction>> StoreActionsFromEvents(IEnumerable<TrackedUserEvent> events, bool saveChanges = false)
        {
            var actions = new List<TrackedUserAction>();
            foreach (var a in events.ToActions())
            {
                actions.Add(await actionStore.Create(a));
            }

            if (saveChanges)
            {
                await storageContext.SaveChanges();
            }

            return actions;
        }

        public async Task<IEnumerable<string>> ReadUniqueActionNames(string commonUserId)
        {
            return await actionStore.ReadUniqueActionNames(commonUserId);
        }

        public async Task<TrackedUserAction> ReadLatestAction(string commonUserId, string actionName)
        {
            return await actionStore.ReadLatestAction(commonUserId, actionName);
        }
    }
}
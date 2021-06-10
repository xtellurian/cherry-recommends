using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class SegmentWorkflows : IWorkflow
    {
        private readonly ILogger<SegmentWorkflows> logger;
        private readonly ISegmentStore segmentStore;
        private readonly ITrackedUserStore userStore;
        private readonly IStorageContext storageContext;

        public SegmentWorkflows(ILogger<SegmentWorkflows> logger,
                                ISegmentStore segmentStore,
                                ITrackedUserStore userStore,
                                IStorageContext storageContext)
        {
            this.logger = logger;
            this.segmentStore = segmentStore;
            this.userStore = userStore;
            this.storageContext = storageContext;
        }
        public async Task<Segment> CreateSegment(string name)
        {
            logger.LogDebug($"Creating Segment with name {name}");
            var segment = await segmentStore.Create(new Segment(name));
            logger.LogDebug($"Created Segment with id {segment.Id}");
            await storageContext.SaveChanges();
            return segment;
        }

        public Task ProcessRule(Rule rule, List<TrackedUserEvent> events)
        {
            throw new System.NotImplementedException();
            // var segment = await segmentStore.Read(rule.SegmentId);
            // var ruleEvents = events.Where(_ => _.Key == rule.EventKey);
            // if (ruleEvents.Any())
            // {
            //     await ProcessEventsForSegmentingRule(segment, ruleEvents);
            //     await segmentStore.Update(segment);
            //     await storageContext.SaveChanges();
            // }
        }

        private async Task ProcessEventsForSegmentingRule(Segment segment, IEnumerable<TrackedUserEvent> events)
        {
            foreach (var e in events)
            {
                var trackedUser = await userStore.ReadFromCommonId(e.CommonUserId);
                if (trackedUser == null)
                {
                    // means the user doesn't exist yet. create them.
                    trackedUser = await userStore.Create(new TrackedUser(e.CommonUserId));
                }

                segment.InSegment.Add(trackedUser);
            }
        }
    }
}
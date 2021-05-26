using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class TrackedUserEventsWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ITrackedUserStore userStore;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserEventStore trackedUserEventStore;

        public TrackedUserEventsWorkflows(IStorageContext storageContext,
                                          IDateTimeProvider dateTimeProvider,
                                          ITrackedUserStore userStore,
                                          IIntegratedSystemStore integratedSystemStore,
                                          ITrackedUserEventStore trackedUserEventStore)
        {
            this.storageContext = storageContext;
            this.dateTimeProvider = dateTimeProvider;
            this.userStore = userStore;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserEventStore = trackedUserEventStore;
        }

        public async Task<IEnumerable<TrackedUserEvent>> TrackUserEvents(IEnumerable<TrackedUserEventInput> input)
        {
            var events = new List<TrackedUserEvent>();
            var newUsers = await userStore.CreateIfNotExists(input.Select(_ => _.CommonUserId).Distinct());

            foreach (var d in input)
            {
                IntegratedSystem sourceSystem = null;
                if (d.SourceSystemId != null)
                {
                    sourceSystem = await integratedSystemStore.Read(d.SourceSystemId.Value);
                }
                events.Add(new TrackedUserEvent(d.CommonUserId,
                                                d.EventId,
                                                d.Timestamp ?? dateTimeProvider.Now,
                                                sourceSystem,
                                                d.Kind,
                                                d.EventType,
                                                d.Properties));
            }


            var results = await trackedUserEventStore.AddTrackedUserEvents(events);
            await storageContext.SaveChanges();
            return results;
        }

#nullable enable
        public struct TrackedUserEventInput
        {
            public TrackedUserEventInput(string commonUserId,
                                         string eventId,
                                         DateTimeOffset? timestamp,
                                         long? sourceSystemId,
                                         string kind,
                                         string eventType,
                                         Dictionary<string, object> properties)
            {
                CommonUserId = commonUserId;
                EventId = eventId;
                Timestamp = timestamp;
                SourceSystemId = sourceSystemId;
                Kind = kind;
                EventType = eventType;
                Properties = new DynamicPropertyDictionary(properties);
            }

            public string CommonUserId { get; set; }
            public string EventId { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public long? SourceSystemId { get; set; }
            public string Kind { get; set; }
            public string EventType { get; set; }
            public DynamicPropertyDictionary Properties { get; set; }
        }
#nullable disable
    }
}
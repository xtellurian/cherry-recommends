using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class TrackedUserEventsWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<TrackedUserEventsWorkflows> logger;
        private readonly ITrackedUserStore userStore;
        private readonly IQueueMessagesFileStore fileStore;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserEventStore trackedUserEventStore;
        private readonly ITrackedUserEventQueueStore eventQueueStore;
        private readonly INewTrackedUserEventQueueStore newTrackedUserQueue;

        public JsonSerializerOptions SerializerOptions => new JsonSerializerOptions();
        public TrackedUserEventsWorkflows(IStorageContext storageContext,
                                          IDateTimeProvider dateTimeProvider,
                                          ILogger<TrackedUserEventsWorkflows> logger,
                                          ITrackedUserStore userStore,
                                          IQueueMessagesFileStore fileStore,
                                          IIntegratedSystemStore integratedSystemStore,
                                          ITrackedUserEventStore trackedUserEventStore,
                                          ITrackedUserEventQueueStore eventQueueStore,
                                          INewTrackedUserEventQueueStore newTrackedUserQueue)
        {
            this.storageContext = storageContext;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
            this.userStore = userStore;
            this.fileStore = fileStore;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserEventStore = trackedUserEventStore;
            this.eventQueueStore = eventQueueStore;
            this.newTrackedUserQueue = newTrackedUserQueue;
        }

        public async Task<EventLoggingResponse> TrackUserEvents(IEnumerable<TrackedUserEventInput> input, bool addToQueue = false, bool ensureCreated = false)
        {
            if (await eventQueueStore.IsWriteEnabled() && addToQueue)
            {
                var json = JsonSerializer.Serialize(input, SerializerOptions);
                var fileName = "TrackedUserEventsWorkflows_TrackedUserEvents_" + dateTimeProvider.Now.ToUnixTimeMilliseconds() + ".json";
                // save to the blob store, then drop message into queue
                await fileStore.WriteFile(json, fileName);
                // save the wholet

                await eventQueueStore.Enqueue(new TrackedUserEventsQueueMessage(fileName));
                await newTrackedUserQueue.Enqueue(new NewTrackedUserEventQueueMessage(input.Select(_ => _.CommonUserId).Distinct()));
                return new EventLoggingResponse { Enqueued = input.Count() };
            }
            else
            {
                logger.LogWarning("Not enqueuing. Prcessing events directly.");
                var events = new List<TrackedUserEvent>();
                var lastUpdated = dateTimeProvider.Now;
                if (ensureCreated)
                {
                    var newUsers = await userStore.CreateIfNotExists(input.Select(_ => _.CommonUserId).Distinct());
                    foreach (var u in newUsers)
                    {
                        u.LastUpdated = lastUpdated;
                    }
                }

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
                return new EventLoggingResponse { Processed = input.Count() }; ;
            }

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
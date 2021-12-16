using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class CustomerEventsWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<CustomerEventsWorkflows> logger;
        private readonly ICustomerStore userStore;
        private readonly IQueueMessagesFileStore fileStore;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ICustomerEventStore trackedUserEventStore;
        private readonly ITrackedUserEventQueueStore eventQueueStore;
        private readonly INewTrackedUserEventQueueStore newTrackedUserQueue;

        public JsonSerializerOptions SerializerOptions => new JsonSerializerOptions();
        public CustomerEventsWorkflows(IStorageContext storageContext,
                                          IDateTimeProvider dateTimeProvider,
                                          ILogger<CustomerEventsWorkflows> logger,
                                          ICustomerStore userStore,
                                          IQueueMessagesFileStore fileStore,
                                          IIntegratedSystemStore integratedSystemStore,
                                          ICustomerEventStore trackedUserEventStore,
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

        public async Task<EventLoggingResponse> AddEvents(IEnumerable<CustomerEventInput> input, bool addToQueue = false)
        {
            if (await eventQueueStore.IsWriteEnabled() && addToQueue)
            {
                var json = JsonSerializer.Serialize(input, SerializerOptions);
                var fileName = "TrackedUserEventsWorkflows_TrackedUserEvents_" + dateTimeProvider.Now.ToUnixTimeMilliseconds() + ".json";
                // save to the blob store, then drop message into queue
                await fileStore.WriteFile(json, fileName);
                // save the wholet

                await eventQueueStore.Enqueue(new TrackedUserEventsQueueMessage(fileName));
                // check the amount of users per message isn't too big (azure throws at messages > 64kB)
                var uniqueUserIds = input.Select(_ => _.CustomerId).Distinct();
                foreach (var uIds in uniqueUserIds.Batch(512))
                {
                    await newTrackedUserQueue.Enqueue(new NewTrackedUserEventQueueMessage(uIds));
                }
                return new EventLoggingResponse { EventsEnqueued = input.Count() };
            }
            else
            {
                logger.LogWarning("Not enqueuing. Prcessing events directly.");
                var events = new List<CustomerEvent>();
                var lastUpdated = dateTimeProvider.Now;
                var customers = await userStore.CreateIfNotExists(input.Select(_ => _.CustomerId).Distinct());
                if (customers.Any())
                {
                    foreach (var u in customers)
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
                    var customer = customers.First(_ => _.CommonId == d.CustomerId);
                    customer.LastUpdated = dateTimeProvider.Now; // user has been updated.
                    events.Add(new CustomerEvent(customer,
                                                    d.EventId,
                                                    d.Timestamp ?? dateTimeProvider.Now,
                                                    sourceSystem,
                                                    d.Kind,
                                                    d.EventType,
                                                    d.Properties,
                                                    d.RecommendationCorrelatorId));
                }


                var results = await trackedUserEventStore.AddRange(events);
                await storageContext.SaveChanges();
                return new EventLoggingResponse { EventsProcessed = input.Count() };
            }

        }

#nullable enable
        public struct CustomerEventInput
        {
            public CustomerEventInput(string customerId,
                                         string eventId,
                                         DateTimeOffset? timestamp,
                                         long? recommendationCorrelatorId,
                                         long? sourceSystemId,
                                         EventKinds kind,
                                         string eventType,
                                         Dictionary<string, object> properties)
            {
                CustomerId = customerId;
                EventId = eventId;
                Timestamp = timestamp;
                RecommendationCorrelatorId = recommendationCorrelatorId;
                SourceSystemId = sourceSystemId;
                Kind = kind;
                EventType = eventType;
                Properties = new DynamicPropertyDictionary(properties);
            }

            public string CustomerId { get; set; }
            public string EventId { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public long? RecommendationCorrelatorId { get; set; }
            public long? SourceSystemId { get; set; }
            public EventKinds Kind { get; set; }
            public string EventType { get; set; }
            public DynamicPropertyDictionary Properties { get; set; }
        }
#nullable disable
    }
}
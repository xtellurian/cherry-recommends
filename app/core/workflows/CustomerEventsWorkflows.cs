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
        private readonly IEnvironmentProvider environmentProvider;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ICustomerEventStore trackedUserEventStore;
        private readonly IEventIngestor eventIngestor;

        public JsonSerializerOptions SerializerOptions => new JsonSerializerOptions();
        public CustomerEventsWorkflows(IStorageContext storageContext,
                                          IDateTimeProvider dateTimeProvider,
                                          ILogger<CustomerEventsWorkflows> logger,
                                          ICustomerStore userStore,
                                          IEnvironmentProvider environmentProvider,
                                          IIntegratedSystemStore integratedSystemStore,
                                          ICustomerEventStore trackedUserEventStore,
                                          IEventIngestor eventIngestor)
        {
            this.storageContext = storageContext;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
            this.userStore = userStore;
            this.environmentProvider = environmentProvider;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserEventStore = trackedUserEventStore;
            this.eventIngestor = eventIngestor;
        }

        public async Task<EventLoggingResponse> Ingest(IEnumerable<CustomerEventInput> input)
        {
            // ingest by default
            if (eventIngestor.CanIngest)
            {
                await eventIngestor.Ingest(input);
                return new EventLoggingResponse
                {
                    EventsEnqueued = input.Count()
                };
            }
            else
            {
                // process directly
                logger.LogWarning("Event Ingestor is not available for ingestion. Processing events directly.");
                return await ProcessEvents(input);
            }
        }
        public async Task<EventLoggingResponse> ProcessEvents(IEnumerable<CustomerEventInput> input)
        {

            logger.LogWarning("Not enqueuing. Prcessing events directly.");
            var events = new List<CustomerEvent>();
            var lastUpdated = dateTimeProvider.Now;
            var customers = await userStore.CreateIfNotExists(input.Select(_ => new PendingCustomer(_.CustomerId, _.EnvironmentId)).Distinct());
            if (customers.Any())
            {
                foreach (var u in customers)
                {
                    u.LastUpdated = lastUpdated;
                }
            }

            foreach (var d in input)
            {
                // make sure you set this before loading the integrated system.
                if (d.EnvironmentId != null)
                {
                    environmentProvider.SetOverride(d.EnvironmentId.Value);
                }

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

#nullable enable
        public struct CustomerEventInput
        {
            public CustomerEventInput(string tenantName,
                                      string customerId,
                                      string eventId,
                                      DateTimeOffset? timestamp,
                                      long? environmentId,
                                      long? recommendationCorrelatorId,
                                      long? sourceSystemId,
                                      EventKinds kind,
                                      string eventType,
                                      Dictionary<string, object> properties)
            {
                TenantName = tenantName;
                CustomerId = customerId;
                EventId = eventId;
                Timestamp = timestamp;
                EnvironmentId = environmentId; // because events aren't using the same EFStoreBase hierarchy
                RecommendationCorrelatorId = recommendationCorrelatorId;
                SourceSystemId = sourceSystemId;
                Kind = kind;
                EventType = eventType;
                Properties = new DynamicPropertyDictionary(properties);
            }

            public string TenantName { get; set; } // required for sending messages to dotnetFunctions
            public string CustomerId { get; set; }
            public string EventId { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public long? EnvironmentId { get; set; }
            public long? RecommendationCorrelatorId { get; set; }
            public long? SourceSystemId { get; set; }
            public EventKinds Kind { get; set; }
            public string EventType { get; set; }
            public DynamicPropertyDictionary Properties { get; set; }
        }
#nullable disable
    }
}
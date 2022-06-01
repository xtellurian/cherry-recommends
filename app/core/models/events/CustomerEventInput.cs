using System;
using System.Collections.Generic;

namespace SignalBox.Core
{
#nullable enable
    public struct CustomerEventInput : IIngestableEvent
    {
        public CustomerEventInput(string tenantName,
                                  string customerId,
                                  string? businessCommonId,
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
            BusinessCommonId = businessCommonId;
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
        public string? BusinessCommonId { get; set; }
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
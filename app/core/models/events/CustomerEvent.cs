using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class CustomerEvent : EnvironmentScopedEntity
    {
        public const string FOUR2_INTERNAL_PREFIX = "_f2_internal_use_";
        public static string FEEDBACK = $"{FOUR2_INTERNAL_PREFIX}_feedback";

        protected CustomerEvent()
        { }
        public CustomerEvent(Customer customer,
                                string eventId,
                                DateTimeOffset timestamp,
                                IntegratedSystem source,
                                EventKinds kind,
                                string eventType,
                                IDictionary<string, object> properties,
                                long? recommendationCorrelatorId = null)
        {
            Customer = customer ?? throw new NullReferenceException("Cannot create an event with a null tracked user");
            CustomerId = customer.CommonId;
            EventId = eventId;
            Timestamp = timestamp;
            Source = source;
            EventKind = kind;
            Kind = kind.ToString();
            EventType = eventType;
            Properties = new DynamicPropertyDictionary(properties);
            RecommendationCorrelatorId = recommendationCorrelatorId;
        }

        public string CommonUserId => CustomerId;
        public string CustomerId { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
# nullable enable
        public long? RecommendationCorrelatorId { get; set; }
        public RecommendationCorrelator? RecommendationCorrelator { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IntegratedSystem? Source { get; set; }
#nullable disable

        public EventKinds? EventKind { get; set; }
        [JsonIgnore]
        public string Kind { get; protected set; }
        public string EventType { get; set; }
        public DynamicPropertyDictionary Properties { get; set; }
        public long? TrackedUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer TrackedUser => Customer;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer Customer { get; set; }
    }
}
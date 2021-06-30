using System;
using System.Collections.Generic;

namespace SignalBox.Core
{
    public class TrackedUserEvent : Entity
    {
        public TrackedUserEvent()
        {
        }

        public TrackedUserEvent(string commonUserId,
                                string eventId,
                                DateTimeOffset timestamp,
                                IntegratedSystem source,
                                string kind,
                                string eventType,
                                IDictionary<string, object> properties,
                                long? recommendationCorrelatorId = null)
        {
            CommonUserId = commonUserId;
            EventId = eventId;
            Timestamp = timestamp;
            Source = source;
            Kind = kind;
            EventType = eventType;
            Properties = new DynamicPropertyDictionary(properties);
            RecommendationCorrelatorId = recommendationCorrelatorId;
        }

        public string CommonUserId { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
# nullable enable
        public long? RecommendationCorrelatorId { get; set; }
        public IntegratedSystem? Source { get; set; }
#nullable disable
        public string Kind { get; set; }
        public string EventType { get; set; }
        public DynamicPropertyDictionary Properties { get; set; }
    }
}
using System;
using System.Collections.Generic;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class TrackedUserEvent : Entity
    {
        public const string FOUR2_INTERNAL_PREFIX = "_f2_internal_use_";
        public static string FEEDBACK = $"{FOUR2_INTERNAL_PREFIX}_feedback";
        protected TrackedUserEvent()
        {
        }

        public TrackedUserEvent(TrackedUser trackedUser,
                                string eventId,
                                DateTimeOffset timestamp,
                                IntegratedSystem source,
                                string kind,
                                string eventType,
                                IDictionary<string, object> properties,
                                long? recommendationCorrelatorId = null)
        {
            if (trackedUser == null)
            {
                throw new NullReferenceException("Cannot create an event with a null tracked user");
            }
            TrackedUser = trackedUser;
            CommonUserId = trackedUser.CommonId;
            EventId = eventId;
            Timestamp = timestamp;
            Source = source;
            Kind = kind;
            EventType = eventType;
            Properties = new DynamicPropertyDictionary(properties);
            RecommendationCorrelatorId = recommendationCorrelatorId;
        }

        public void AddGoodFeedback()
        {
            this.Properties ??= new DynamicPropertyDictionary();
            this.Properties[FEEDBACK] = 0.8;
        }
        public void AddBadFeedback()
        {
            this.Properties ??= new DynamicPropertyDictionary();
            this.Properties[FEEDBACK] = -0.4;
        }

        public string CommonUserId { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
# nullable enable
        public long? RecommendationCorrelatorId { get; set; }
        public RecommendationCorrelator? RecommendationCorrelator { get; set; }
        public IntegratedSystem? Source { get; set; }
#nullable disable
        public string Kind { get; set; }
        public string EventType { get; set; }
        public DynamicPropertyDictionary Properties { get; set; }
        public long? TrackedUserId { get; set; }
        public TrackedUser TrackedUser { get; set; }
        public ICollection<TrackedUserAction> Actions { get; set; }
    }
}
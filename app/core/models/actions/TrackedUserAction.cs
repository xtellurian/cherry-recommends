using System;

namespace SignalBox.Core
{
    // a tracked user action is a slice of an event
    // an event with N Properties is split into N Actions
    public class TrackedUserAction : Entity
    {
        protected TrackedUserAction()
        { }
        private TrackedUserAction(string commonUserId,
                                 string eventId,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string actionName)
        {
            CommonUserId = commonUserId;
            EventId = eventId;
            Timestamp = timestamp;
            RecommendationCorrelatorId = recommendationCorrelatorId;
            IntegratedSystemId = integratedSystemId;
            Category = category;
            ActionName = actionName;
        }
        public TrackedUserAction(string commonUserId,
                                 string eventId,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string property,
                                 string value)
        : this(commonUserId, eventId, timestamp, recommendationCorrelatorId, integratedSystemId, category, property)
        {
            ActionValue = value;
            ValueType = TrackedUserActionValueType.String;
        }

        public TrackedUserAction(string commonUserId,
                                 string eventId,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string property,
                                 double value)
        : this(commonUserId, eventId, timestamp, recommendationCorrelatorId, integratedSystemId, category, property)
        {
            ActionValue = value.ToString();
            ValueType = TrackedUserActionValueType.Float;
        }
        public TrackedUserAction(string commonUserId,
                                 string eventId,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string actionName,
                                 int value)
        : this(commonUserId, eventId, timestamp, recommendationCorrelatorId, integratedSystemId, category, actionName)
        {
            ActionValue = value.ToString();
            ValueType = TrackedUserActionValueType.Int;
        }

        public string CommonUserId { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long? RecommendationCorrelatorId { get; set; }
        public long? IntegratedSystemId { get; set; }

        // category can be bar separated Kind|EventType
        public string Category { get; set; }
        public string ActionName { get; set; }
        public string ActionValue { get; set; }
        public TrackedUserActionValueType ValueType { get; set; }

    }
}
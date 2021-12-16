using System;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    // a tracked user action is a slice of an event
    // an event with N Properties is split into N Actions
    public class TrackedUserAction : Entity
    {
        protected TrackedUserAction()
        { }
        private TrackedUserAction(Customer customer,
                                 CustomerEvent trackedUserEvent,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string actionName)
        {
            if (customer == null)
            {
                throw new System.NullReferenceException("Tracked User cannot be null for an event.");
            }
            CustomerId = customer.CommonId;
            Customer = customer;
            EventId = trackedUserEvent?.EventId ?? "auto-" + Guid.NewGuid().ToString();
            TrackedUserEvent = trackedUserEvent;
            Timestamp = timestamp;
            RecommendationCorrelatorId = recommendationCorrelatorId;
            IntegratedSystemId = integratedSystemId;
            Category = category;
            ActionName = actionName;
        }
        public TrackedUserAction(Customer customer,
                                 CustomerEvent trackedUserEvent,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string property,
                                 string value)
        : this(customer, trackedUserEvent, timestamp, recommendationCorrelatorId, integratedSystemId, category, property)
        {
            ActionValue = value;
            ValueType = TrackedUserActionValueType.String;
        }

        public TrackedUserAction(Customer customer,
                                 CustomerEvent trackedUserEvent,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string property,
                                 double value)
        : this(customer, trackedUserEvent, timestamp, recommendationCorrelatorId, integratedSystemId, category, property)
        {
            ActionValue = value.ToString();
            ValueType = TrackedUserActionValueType.Float;
        }
        public TrackedUserAction(Customer customer,
                                 CustomerEvent trackedUserEvent,
                                 DateTimeOffset timestamp,
                                 long? recommendationCorrelatorId,
                                 long? integratedSystemId,
                                 string category,
                                 string actionName,
                                 int value)
        : this(customer, trackedUserEvent, timestamp, recommendationCorrelatorId, integratedSystemId, category, actionName)
        {
            ActionValue = value.ToString();
            ValueType = TrackedUserActionValueType.Int;
        }

        public long? TrackedUserId { get; set; }
        // API Backwards Compat.
        public Customer TrackedUser => Customer;
        public Customer Customer { get; set; }
        public string CommonUserId => CustomerId;
        public string CustomerId { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long? RecommendationCorrelatorId { get; set; }
        public RecommendationCorrelator RecommendationCorrelator { get; set; }
        public long? IntegratedSystemId { get; set; }

        // category can be bar separated Kind|EventType
        public string Category { get; set; }
        public string ActionName { get; set; }
        public string ActionValue { get; set; }
        public TrackedUserActionValueType ValueType { get; set; }

        [JsonIgnore] // ignore to prevent circular serialization problem
        public CustomerEvent TrackedUserEvent { get; set; }
        public long? TrackedUserEventId { get; set; }
        // rewards
        public bool HasReward() => this.AssociatedRevenue != null || this.FeedbackScore != null;
        public double? FeedbackScore { get; set; }
        public double? AssociatedRevenue { get; set; }
    }
}
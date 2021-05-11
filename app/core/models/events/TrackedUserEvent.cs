using System;

namespace SignalBox.Core
{
    // These have either a numeric or logical value
    // A set of them exist at any given time.
#nullable enable
    public class TrackedUserEvent
    {
        // these are keys that are special or common
        public static string ExperimentParticipationKey = "Experiment Participation"; // logical value should be experiment id
        public static string OfferPresentedKey = "Offer Presented"; // logical value should be offer id
        public static string OfferAcceptedKey = "Offer Accepted"; // logical value should be offer id
        public static string OfferRejectedKey = "Offer Rejected"; // logical value should be offer id

        // when a user participates in an experiment, then there should be a property with
        // key = "Experiment Participation" and LogicalValue = <the experiment Id>
        public TrackedUserEvent(string trackedUserExternalId, DateTimeOffset timestamp, string key, string? logicalValue, double? numericValue = null)
        {
            TrackedUserExternalId = trackedUserExternalId;
            Timestamp = timestamp;
            Key = key;
            if (logicalValue != null && numericValue != null)
            {
                throw new ArgumentException("Property cannot have noth numeric and logical values");
            }
            else if (logicalValue != null)
            {
                LogicalValue = logicalValue;
                Kind = TrackedUserPropertyKind.Logical;
            }
            else if (numericValue != null)
            {
                NumericValue = numericValue;
                Kind = TrackedUserPropertyKind.Numeric;
            }
            else
            {
                throw new ArgumentException("TrackedUserEvent must have a logical or numeric value");
            }
        }

        public static TrackedUserEvent FromEventParticipation(string trackedUserId, DateTimeOffset timestamp, Experiment experiment)
        {
            return new TrackedUserEvent(trackedUserId, timestamp, ExperimentParticipationKey, experiment.Id.ToString());
        }
        public static TrackedUserEvent FromOfferAccepted(string trackedUserId, DateTimeOffset timestamp, Offer offer)
        {
            return new TrackedUserEvent(trackedUserId, timestamp, OfferAcceptedKey, offer.Id.ToString());
        }
        public static TrackedUserEvent FromOfferRejected(string trackedUserId, DateTimeOffset timestamp, Offer offer)
        {
            return new TrackedUserEvent(trackedUserId, timestamp, OfferRejectedKey, offer.Id.ToString());
        }
        public static TrackedUserEvent FromOfferPresented(string trackedUserId, DateTimeOffset timestamp, Offer offer)
        {
            return new TrackedUserEvent(trackedUserId, timestamp, OfferPresentedKey, offer.Id.ToString());
        }

        public string Id { get; set; } = Guid.NewGuid().ToString(); // default to new ID
        public string TrackedUserExternalId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public TrackedUserPropertyKind Kind { get; set; }
        public string Key { get; set; }
        public string? LogicalValue { get; set; }
        public double? NumericValue { get; set; }
    }

    public enum TrackedUserPropertyKind
    {
        Logical,
        Numeric

    }
}
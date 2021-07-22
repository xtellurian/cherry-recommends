using System;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public class InvokationLogEntry : Entity
    {
        protected InvokationLogEntry() { }
        public InvokationLogEntry(RecommenderEntityBase recommender, DateTimeOffset started, string message)
        {
            RecommenderId = recommender.Id;
            RecommenderType = recommender.GetType().Name;
            Message = message;
            InvokeStarted = started;
        }

#nullable enable
        public string RecommenderType { get; set; }
        public long RecommenderId { get; set; }
        public bool? Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ModelResponse { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
        public DateTimeOffset InvokeStarted { get; set; }
        public DateTimeOffset? InvokeEnded { get; set; }
        public long? CorrelatorId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RecommendationCorrelator? Correlator { get; set; }
        public long? TrackedUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TrackedUser? TrackedUser { get; set; }
    }
}
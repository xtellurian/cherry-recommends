using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public class InvokationLogEntry : Entity
    {
        protected InvokationLogEntry() { }
        public InvokationLogEntry(RecommenderEntityBase recommender, DateTimeOffset started)
        {
            RecommenderId = recommender.Id;
            RecommenderType = recommender.GetType().Name;
            InvokeStarted = started;
            Status = "Started";
        }

        public void LogMessage(string message)
        {
            Messages ??= new List<string>();
            if (message != null)
            {
                this.Messages.Add(message);
            }
        }

#nullable enable
        public string? Status { get; set; }
        public string RecommenderType { get; set; }
        public long RecommenderId { get; set; }
        public bool? Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ModelResponse { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Obsolete]
        public string? Message { get; set; }
        public List<string>? Messages { get; set; } = new List<string>();
        public DateTimeOffset InvokeStarted { get; set; }
        public DateTimeOffset? InvokeEnded { get; set; }
        public long? CorrelatorId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RecommendationCorrelator? Correlator { get; set; }
        public long? TrackedUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer? Customer { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer? TrackedUser => Customer;
    }
}
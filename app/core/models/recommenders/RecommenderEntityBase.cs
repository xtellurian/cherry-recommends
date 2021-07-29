using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Recommenders
{
    public abstract class RecommenderEntityBase : CommonEntity, IRecommender
    {
        protected RecommenderEntityBase()
        { }
        public RecommenderEntityBase(string commonId, string name, RecommenderErrorHandling errorHandling) : base(commonId, name)
        {
            this.ErrorHandling = errorHandling ?? new RecommenderErrorHandling();
        }

#nullable enable

        public bool ShouldThrowOnBadInput() => this.ErrorHandling?.ThrowOnBadInput == true;
        public RecommenderErrorHandling? ErrorHandling { get; set; }
        public long? ModelRegistrationId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ModelRegistration? ModelRegistration { get; set; }
        [JsonIgnore]
        public ICollection<RecommenderTargetVariableValue> TargetVariableValues { get; set; }
        [JsonIgnore]
        public ICollection<InvokationLogEntry> RecommenderInvokationLogs { get; set; }
    }
}
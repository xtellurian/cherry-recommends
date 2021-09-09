using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public abstract class RecommenderEntityBase : CommonEntity, IRecommender
    {
        protected RecommenderEntityBase()
        { }
#nullable enable
        public RecommenderEntityBase(string commonId,
                                     string? name,
                                     IEnumerable<RecommenderArgument>? arguments,
                                     RecommenderErrorHandling? errorHandling) : base(commonId, name)
        {
            this.ErrorHandling = errorHandling ?? new RecommenderErrorHandling();
            this.Arguments = arguments?.ToList() ?? new List<RecommenderArgument>();
        }


        public bool ShouldThrowOnBadInput() => this.ErrorHandling?.ThrowOnBadInput == true;
        public RecommenderErrorHandling? ErrorHandling { get; set; }
        public List<RecommenderArgument>? Arguments { get; set; }
        public long? ModelRegistrationId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ModelRegistration? ModelRegistration { get; set; }


        [JsonIgnore]
        public ICollection<RecommenderTargetVariableValue> TargetVariableValues { get; set; } = null!;
        [JsonIgnore]
        public ICollection<InvokationLogEntry> RecommenderInvokationLogs { get; set; } = null!;
        [JsonIgnore]
        public ICollection<RecommendationCorrelator> RecommendationCorrelators { get; set; } = null!;
    }
}
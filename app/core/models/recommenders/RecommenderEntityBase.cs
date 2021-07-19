using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Recommenders
{
    public abstract class RecommenderEntityBase : CommonEntity, IRecommender
    {
        public RecommenderEntityBase()
        { }
        public RecommenderEntityBase(string commonId, string name) : base(commonId, name) { }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ModelRegistration ModelRegistration { get; set; }
        [JsonIgnore]
        public ICollection<RecommenderTargetVariableValue> TargetVariableValues { get; set; }
        [JsonIgnore]
        public ICollection<InvokationLogEntry> RecommenderInvokationLogs { get; set; }
    }
}
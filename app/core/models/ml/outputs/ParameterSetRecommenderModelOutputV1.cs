using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class ParameterSetRecommenderModelOutputV1 : IModelOutput
    {
        public ParameterSetRecommenderModelOutputV1()
        { }

        public Dictionary<string, object> RecommendedParameters { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? CorrelatorId { get; set; }
    }
}
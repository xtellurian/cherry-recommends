using System.Collections.Generic;

namespace SignalBox.Core
{
    public class ParameterSetRecommenderModelOutputV1 : IModelOutput
    {
        public ParameterSetRecommenderModelOutputV1()
        { }

        public Dictionary<string, object> RecommendedParameters { get; set; }
        public long? CorrelatorId { get; set; }
    }
}
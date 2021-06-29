using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
#nullable enable
    public class ParameterSetRecommenderModelInputV1 : IModelInput
    {
        [Required]
        public Dictionary<string, object> Arguments { get; set; } = null!;
        [Required]
        public long? ParameterSetRecommenderId { get; set; }
        public List<ParameterBounds>? ParameterBounds { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
#nullable enable
    public class ParameterSetRecommenderModelInputV1 : IModelInput
    {
        public string? CommonUserId { get; set; }
        [Required]
        public Dictionary<string, object> Arguments { get; set; } = null!;
        public List<ParameterBounds>? ParameterBounds { get; set; }
    }
}
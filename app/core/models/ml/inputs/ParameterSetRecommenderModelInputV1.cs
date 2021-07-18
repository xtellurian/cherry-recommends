using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
#nullable enable
    public class ParameterSetRecommenderModelInputV1 : IModelInput
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string? CommonUserId { get; set; } = System.Guid.NewGuid().ToString(); // create a new GUID here if empty
        [Required]
        public Dictionary<string, object> Arguments { get; set; } = null!;
        public List<ParameterBounds>? ParameterBounds { get; set; }
    }
}
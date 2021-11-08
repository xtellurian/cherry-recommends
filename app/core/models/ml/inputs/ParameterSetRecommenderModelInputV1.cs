using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
#nullable enable
    public class ParameterSetRecommenderModelInputV1 : IModelInput
    {
        public ParameterSetRecommenderModelInputV1()
        { }
        public ParameterSetRecommenderModelInputV1(string commonUserId)
        {
            this.CommonUserId = commonUserId;
        }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string? CommonUserId { get; set; } = System.Guid.NewGuid().ToString(); // create a new GUID here if empty
        [Required]
        public IDictionary<string, object>? Arguments { get; set; } = null!;
        public IDictionary<string, object>? Features { get; set; }
        public IEnumerable<ParameterBounds>? ParameterBounds { get; set; }
    }
}
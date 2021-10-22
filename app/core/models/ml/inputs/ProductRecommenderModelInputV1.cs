using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class ProductRecommenderModelInputV1 : IModelInput
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string CommonUserId { get; set; }
        public string Version { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
        public IDictionary<string, object> Features { get; set; }
        public IEnumerable<ParameterBounds> ParameterBounds { get; set; }
    }
}
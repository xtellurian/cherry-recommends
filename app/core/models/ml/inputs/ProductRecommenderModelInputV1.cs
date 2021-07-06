using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Core
{
    public class ProductRecommenderModelInputV1 : IModelInput
    {
        [Required]
        public string CommonUserId { get; set; }
        public string Version { get; set; }
        public string Touchpoint { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
    }
}
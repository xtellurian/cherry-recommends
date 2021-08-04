using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto.RecommenderInputs
{
    public class ProductRecommenderInput
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string CommonUserId { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
    }
}
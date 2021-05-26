using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable enable
namespace SignalBox.Web.Dto
{
    public class RecommendationContextDto : DtoBase
    {
        public RecommendationContextDto(string commonUserId, Dictionary<string, object>? features)
        {
            CommonUserId = commonUserId;
            Features = features;
        }

        [Required]
        public string CommonUserId { get; set; }
        public Dictionary<string, object>? Features { get; set; }
    }
}
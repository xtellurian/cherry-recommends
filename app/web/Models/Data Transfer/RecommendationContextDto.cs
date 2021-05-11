using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable enable
namespace SignalBox.Web.Dto
{
    public class RecommendationContextDto : DtoBase
    {
        public RecommendationContextDto(string externalTrackedUserId, Dictionary<string, object>? features)
        {
            ExternalTrackedUserId = externalTrackedUserId;
            Features = features;
        }

        [Required]
        public string ExternalTrackedUserId { get; set; }
        public Dictionary<string, object>? Features { get; set; }
    }
}
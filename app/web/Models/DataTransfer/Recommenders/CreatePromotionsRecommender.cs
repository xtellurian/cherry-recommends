using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreatePromotionsRecommender : CreateRecommenderDtoBase
    {
        public string GetBaselinePromotionId()
        {
            var id = BaselinePromotionId ?? BaselineItemId ?? DefaultItemId;
            if (string.IsNullOrEmpty(id))
            {
                throw new BadRequestException("BaselinePromotionId is required");
            }
            return id;
        }
        public IEnumerable<string>? ItemIds { get; set; }
        public string? DefaultItemId { get; set; } // backwards compat only
        public string? BaselineItemId { get; set; } // backwards compat only
        public string? BaselinePromotionId { get; set; }
        public int? NumberOfItemsToRecommend { get; set; }
        public bool? UseAutoAi { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateItemsRecommender : CreateRecommenderDtoBase
    {
        public string GetBaselineItemId() => BaselineItemId ?? DefaultItemId ?? throw new BadRequestException("BaselineItemId is required");
        public IEnumerable<string>? ItemIds { get; set; }
        public string? DefaultItemId { get; set; } // backwards compat only
        public string? BaselineItemId { get; set; }
        public int? NumberOfItemsToRecommend { get; set; }
        public bool? UseAutoAi { get; set; }
    }
}
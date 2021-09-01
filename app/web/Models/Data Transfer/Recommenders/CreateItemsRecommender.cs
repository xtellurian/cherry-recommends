using System.Collections.Generic;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateItemsRecommender : CreateRecommenderDtoBase
    {
        public IEnumerable<string>? ItemIds { get; set; }
        public string? DefaultItemId { get; set; }
        public int? NumberOfItemsToRecommend { get; set; }
    }
}
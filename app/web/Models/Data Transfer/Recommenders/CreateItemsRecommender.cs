using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateItemsRecommender : CreateRecommenderDtoBase
    {
        public IEnumerable<string>? ItemIds { get; set; }
        [Required]
        public string? DefaultItemId { get; set; }
        public int? NumberOfItemsToRecommend { get; set; }
    }
}
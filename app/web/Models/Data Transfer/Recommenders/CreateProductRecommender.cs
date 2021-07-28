using System.Collections.Generic;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateProductRecommender : CreateRecommenderDtoBase
    {
        public IEnumerable<string>? ProductIds { get; set; }
        public string Touchpoint { get; set; } = null!;
        public string? DefaultProductId { get; set; } = null!;
    }
}
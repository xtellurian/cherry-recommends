using System.Collections.Generic;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateProductRecommender : CommonDtoBase
    {
        public IEnumerable<string>? ProductIds { get; set; }
        public string Touchpoint { get; set; } = null!;
    }
}
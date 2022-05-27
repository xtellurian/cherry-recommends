using System.Collections.Generic;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateProductCampaign : CreateCampaignDtoBase
    {
        public IEnumerable<string>? ProductIds { get; set; }
        public string? DefaultProductId { get; set; } = null!;
    }
}
using System.Collections.Generic;
using SignalBox.Core.Campaigns;

namespace SignalBox.Web.Dto
{
    public class CreateParameterSetCampaign : CreateCampaignDtoBase
    {
        public IEnumerable<string> Parameters { get; set; }
        public IEnumerable<ParameterBounds> Bounds { get; set; }
    }
}
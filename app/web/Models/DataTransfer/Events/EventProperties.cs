using System.Collections.Generic;

namespace SignalBox.Web.Dto
{
    public class EventProperties : Dictionary<string, object>
    {
        public long? PromotionId => this.GetValueOrDefault("promotionId") as long?;
        public double? Value => this.GetValueOrDefault("value") as double?;
    }
}
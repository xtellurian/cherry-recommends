using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core.Adapters.Hubspot
{
#nullable enable
    public class HubspotPropertyCollection
    {
        private const int MaxProperties = 20;
        public IEnumerable<string>? PropertyNames { get; set; } = new List<string>();

        public void Validate()
        {
            if (this.PropertyNames?.Count() > MaxProperties)
            {
                throw new BadRequestException($"The number of properties is limited to {MaxProperties}");
            }
        }
    }
}
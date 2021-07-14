using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
    public class HubspotAssociation
    {
        public HubspotAssociation(string id, string type)
        {
            this.Id = id;
            this.Type = type;
        }

        public string Id { get; set; }
        public string Type { get; set; }
    }
}
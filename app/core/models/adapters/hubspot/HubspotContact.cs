using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
    public class HubspotContact
    {
        public HubspotContact(string objectId, IDictionary<string, string> properties)
        {
            ObjectId = objectId;
            Properties = properties;
        }

        public string ObjectId { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}
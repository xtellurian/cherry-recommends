using System;
using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
    public struct HubspotEvent
    {
        public HubspotEvent(string objectId, string objectType, DateTimeOffset occurredAt, IDictionary<string, string> properties)
        {
            ObjectId = objectId;
            ObjectType = objectType;
            OccurredAt = occurredAt;
            Properties = properties;
        }

        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}
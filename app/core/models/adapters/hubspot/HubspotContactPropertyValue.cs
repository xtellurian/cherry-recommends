namespace SignalBox.Core.Adapters.Hubspot
{
    public class HubspotContactPropertyValue
    {
        public HubspotContactPropertyValue(string objectId, string property, string value)
        {
            ObjectId = objectId;
            Property = property.ToLower(); // because all properties must be lowercase
            Value = value;
        }
        public string ObjectId { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
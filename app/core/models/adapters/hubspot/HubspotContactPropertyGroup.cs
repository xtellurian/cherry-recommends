namespace SignalBox.Core.Adapters.Hubspot
{
    public struct HubspotContactPropertyGroup
    {
        public HubspotContactPropertyGroup(string name, string label)
        {
            this.Name = name;
            this.Label = label;
        }
        public string Name { get; set; }
        public string Label { get; set; }
    }
}
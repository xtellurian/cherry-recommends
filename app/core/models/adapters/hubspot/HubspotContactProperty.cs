namespace SignalBox.Core.Adapters.Hubspot
{
    public struct HubspotContactProperty
    {
#nullable enable
        public HubspotContactProperty(string name, string label, string type, string description, bool hubspotDefined, string? groupName)
        {
            Name = name;
            Label = label;
            Type = type;
            Description = description;
            HubspotDefined = hubspotDefined;
            GroupName = groupName;
        }

        public string Name { get; set; } // the internal name
        public string Label { get; set; } // the human readable label
        public string Type { get; set; } // the data type
        public string Description { get; set; } // a description of the property
        public bool HubspotDefined { get; set; } // whether the property is defined by Hubspot
        public string? GroupName { get; set; } // the Hubspot property group name
    }
}
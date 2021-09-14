namespace SignalBox.Core.Integrations.Hubspot
{
#nullable enable
    public class HubspotTrackedUserLinkBehaviour
    {
        public string? CommonUserIdPropertyName { get; set; } = string.Empty; // which field to use as common user ID. if null, use object Id 
        public bool? CreateUserIfNotExist { get; set; } = true;
        public string? PropertyPrefix { get; set; } = string.Empty;
    }
}
namespace SignalBox.Core.Integrations
{
    public class HubspotAppCredentials : IIntegratedSystemCredentials
    {
        public string AppId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
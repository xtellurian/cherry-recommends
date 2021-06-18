using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class HubspotAppInformation
    {
        public HubspotAppInformation(string clientId, string scope)
        {
            ClientId = clientId;
            Scope = scope;
        }

        public string ClientId { get; set; }
        public string Scope { get; set; }
    }
}
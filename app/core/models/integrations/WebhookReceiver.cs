using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class WebhookReceiver : Entity
    {
        public WebhookReceiver()
        { }

        public WebhookReceiver(string endpointId, IntegratedSystem integratedSystem, string sharedSecret = null)
        {
            EndpointId = endpointId;
            SharedSecret = sharedSecret;
            IntegratedSystem = integratedSystem;
        }

        public string EndpointId { get; set; }
        public string SharedSecret { get; set; }

        [JsonIgnore]
        public IntegratedSystem IntegratedSystem { get; set; }
    }
}
namespace SignalBox.Core.Adapters.Hubspot
{
    public class HubspotActionHookResponse
    {
        public HubspotActionHookResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
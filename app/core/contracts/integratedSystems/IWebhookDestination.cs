namespace SignalBox.Core
{
    public interface IWebhookDestination
    {
        string Endpoint { get; }
    }
}
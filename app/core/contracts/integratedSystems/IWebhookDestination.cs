namespace SignalBox.Core
{
#nullable enable
    public interface IWebhookDestination
    {
        string Endpoint { get; }
        string? ApplicationSecret { get; }
    }
}
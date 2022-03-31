using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace SignalBox.Core
{
    public interface IWebhookWorkflow
    {
        Task<EventLoggingResponse> ProcessWebhookRequest(WebhookReceiver receiver, IEnumerable<KeyValuePair<string, StringValues>> headers, string body, string signature = null);
    }
}
using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Files
{
    public class AzureBlobQueueMessagesFileStore : AzureBlobFileStore, IQueueMessagesFileStore
    {
        public AzureBlobQueueMessagesFileStore(IOptions<QueueMessagesFileHosting> options) : base(options)
        {
        }
    }
}
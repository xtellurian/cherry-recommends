using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IQueueStore<T> where T : IQueueMessage
    {
        Task<bool> IsReadEnabled();
        Task<bool> IsWriteEnabled();
        Task CompleteDequeue(System.Collections.Generic.IEnumerable<DequeuedMessage<T>> dequeuedMessages);
        Task Enqueue(T message);
        Task<System.Collections.Generic.IEnumerable<DequeuedMessage<T>>> StartDequeue();
    }
}
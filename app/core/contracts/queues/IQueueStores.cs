using System.Threading.Tasks;

namespace SignalBox.Core
{
    /// <summary>
    /// A single wrapper for all queues to send messages from the same place.
    /// </summary>
    public interface IQueueStores
    {
        /// <summary>
        /// Routes a message to the right queue based on it's type.
        /// </summary>
        /// <typeparam name="T">The type of the message. Will choose the queue based on the type.</typeparam>
        /// <param name="message">The message to send to the queue.</param>
        /// <returns>When completed.</returns>
        Task Enqueue<T>(T message) where T : IQueueMessage;
    }
}
using System.Collections.Generic;

namespace SignalBox.Core
{
    public class DequeuedMessage<T> where T : IQueueMessage
    {
        public DequeuedMessage()
        { }

        public DequeuedMessage(T item, IDictionary<string, string> properties)
        {
            Item = item;
            Properties = properties;
        }

        public T Item { get; set; }
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
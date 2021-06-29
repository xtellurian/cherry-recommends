using System;

namespace SignalBox.Core
{
    public class TrackedUserEventsQueueMessage : IQueueMessage
    {
        public TrackedUserEventsQueueMessage()
        { }
        public TrackedUserEventsQueueMessage(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }
        public string Type => nameof(TrackedUserEventsQueueMessage);
    }
}
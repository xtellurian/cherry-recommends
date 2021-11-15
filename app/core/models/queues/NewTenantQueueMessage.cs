using System;

namespace SignalBox.Core
{
    public class NewTenantQueueMessage : IQueueMessage
    {
        public NewTenantQueueMessage()
        { }
        public NewTenantQueueMessage(string name, string creatorId)
        {
            this.Name = name;
            this.CreatorId = creatorId;
        }

        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string Type => nameof(NewTenantQueueMessage);
    }
}
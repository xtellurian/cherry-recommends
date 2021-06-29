using System;
using System.Collections.Generic;

namespace SignalBox.Core
{
    public class NewTrackedUserEventQueueMessage : IQueueMessage
    {
        public NewTrackedUserEventQueueMessage()
        { }
        public NewTrackedUserEventQueueMessage(IEnumerable<string> commonIds)
        {
            CommonIds = commonIds;
        }

        public IEnumerable<string> CommonIds { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace SignalBox.Core
{
    public class NewCustomerEventQueueMessage : IQueueMessage
    {
        public NewCustomerEventQueueMessage()
        { }
        public NewCustomerEventQueueMessage(IEnumerable<PendingCustomer> messages)
        {
            PendingCustomers = messages;
        }

        public IEnumerable<PendingCustomer> PendingCustomers { get; set; }
    }
}
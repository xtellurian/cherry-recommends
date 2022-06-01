namespace SignalBox.Core
{
    public class CustomerHasUpdated : IIngestableEvent
    {
        public CustomerHasUpdated()
        { }

        public CustomerHasUpdated(string tenantName, Customer customer)
        {
            CustomerId = customer.Id;
            EventId = System.Guid.NewGuid().ToString();
            TenantName = tenantName;
        }

        public long? CustomerId { get; set; }

        public string EventId { get; set; }
        public string TenantName { get; set; } // required for sending messages to dotnetFunctions
    }
}

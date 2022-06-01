namespace SignalBox.Core
{
    public class CustomerHasUpdated : IIngestableEvent
    {
        public CustomerHasUpdated()
        { }

        public CustomerHasUpdated(Customer customer)
        {
            CustomerId = customer.Id;
            EventId = System.Guid.NewGuid().ToString();
        }

        public long? CustomerId { get; set; }

        public string EventId { get; set; }
    }
}

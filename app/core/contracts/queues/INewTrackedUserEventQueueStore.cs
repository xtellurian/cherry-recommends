namespace SignalBox.Core
{
    public interface INewTrackedUserEventQueueStore : IQueueStore<NewCustomerEventQueueMessage>
    { }
}
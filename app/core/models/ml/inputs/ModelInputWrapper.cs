namespace SignalBox.Core
{
    public class ModelInputWrapper<T> where T : IModelInput
    {
        public ModelInputWrapper(T payload, long? correlatorId)
        {
            Payload = payload;
            CorrelatorId = correlatorId;
        }

        public long? CorrelatorId { get; set; }
        public T Payload { get; set; }
    }
}
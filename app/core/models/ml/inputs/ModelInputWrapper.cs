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

        // making this an object ensures every property is serialized
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-polymorphism
        public object Payload { get; set; }
    }
}
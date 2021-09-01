namespace SignalBox.Core
{
    public class ModelInputWrapper<T> where T : IModelInput
    {
        public ModelInputWrapper(string version, T payload, long? correlatorId)
        {
            Version = version;
            Payload = payload;
            CorrelatorId = correlatorId;
        }

        public string Version { get; set; }
        public long? CorrelatorId { get; set; }
        public T Payload { get; set; }
    }
}
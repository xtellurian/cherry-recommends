namespace SignalBox.Core
{
    public class ModelInputWrapper<T> where T : IModelInput
    {
        public ModelInputWrapper(string version, T payload)
        {
            Version = version;
            Payload = payload;
        }

        public string Version { get; set; }
        public T Payload { get; set; }
    }
}
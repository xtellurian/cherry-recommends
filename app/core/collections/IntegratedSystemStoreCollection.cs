namespace SignalBox.Core
{
    public class IntegratedSystemStoreCollection
    {
        public IntegratedSystemStoreCollection(IIntegratedSystemStore integratedSystemStore,
                                               ICustomIntegratedSystemStore customIntegratedSystemStore)
        {
            this.IntegratedSystemStore = integratedSystemStore;
            this.CustomIntegratedSystemStore = customIntegratedSystemStore;
        }

        public IIntegratedSystemStore IntegratedSystemStore { get; private set; }

        public ICustomIntegratedSystemStore CustomIntegratedSystemStore { get; private set; }
    }
}
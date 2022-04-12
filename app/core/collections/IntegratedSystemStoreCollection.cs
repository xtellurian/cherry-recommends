namespace SignalBox.Core
{
    public class IntegratedSystemStoreCollection
    {
        public IntegratedSystemStoreCollection(IIntegratedSystemStore integratedSystemStore,
                                               ICustomIntegratedSystemStore customIntegratedSystemStore,
                                               IWebsiteIntegratedSystemStore websiteIntegratedSystemStore)
        {
            this.IntegratedSystemStore = integratedSystemStore;
            this.CustomIntegratedSystemStore = customIntegratedSystemStore;
            this.WebsiteIntegratedSystemStore = websiteIntegratedSystemStore;
        }

        public IIntegratedSystemStore IntegratedSystemStore { get; private set; }

        public ICustomIntegratedSystemStore CustomIntegratedSystemStore { get; private set; }
        public IWebsiteIntegratedSystemStore WebsiteIntegratedSystemStore { get; private set; }
    }
}
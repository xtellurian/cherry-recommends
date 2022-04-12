namespace SignalBox.Core.Integrations.Website
{
    public class WebsiteIntegratedSystem : IntegratedSystem
    {
        protected WebsiteIntegratedSystem() { }
        public WebsiteIntegratedSystem(string commonId, string name) : base(commonId, name, IntegratedSystemTypes.Website)
        {
            this.ApplicationId = System.Guid.NewGuid().ToString();
            this.ApplicationSecret = System.Guid.NewGuid().ToString().ToBase64Encoded();
        }

        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
    }
}
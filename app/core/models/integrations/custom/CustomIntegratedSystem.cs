namespace SignalBox.Core.Integrations.Custom
{
    public class CustomIntegratedSystem : IntegratedSystem
    {
        protected CustomIntegratedSystem() { }
        public CustomIntegratedSystem(string commonId, string name) : base(commonId, name, IntegratedSystemTypes.Custom)
        {
            this.ApplicationId = System.Guid.NewGuid().ToString();
            this.ApplicationSecret = System.Guid.NewGuid().ToString().ToBase64Encoded();
        }

        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
    }
}
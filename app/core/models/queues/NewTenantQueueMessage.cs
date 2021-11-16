namespace SignalBox.Core
{
    public class NewTenantQueueMessage : IQueueMessage
    {
        public NewTenantQueueMessage()
        { }
        public NewTenantQueueMessage(string name, string creatorId, string termsOfServiceVersion)
        {
            this.Name = name;
            this.CreatorId = creatorId;
            this.TermsOfServiceVersion = termsOfServiceVersion;
        }

        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string TermsOfServiceVersion { get; set; }
        public string Type => nameof(NewTenantQueueMessage);
    }
}
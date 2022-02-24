namespace SignalBox.Core
{
    public class NewTenantQueueMessage : IQueueMessage
    {
        public NewTenantQueueMessage()
        { }
        public NewTenantQueueMessage(string name, string creatorId, string creatorEmail, string termsOfServiceVersion)
        {
            Name = name;
            CreatorId = creatorId;
            CreatorEmail = creatorEmail;
            TermsOfServiceVersion = termsOfServiceVersion;
        }

        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorEmail { get; set; }
        public string TermsOfServiceVersion { get; set; }
        public string Type => nameof(NewTenantQueueMessage);
    }
}
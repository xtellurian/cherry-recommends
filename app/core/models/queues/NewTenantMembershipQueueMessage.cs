namespace SignalBox.Core
{
    public class NewTenantMembershipQueueMessage : IQueueMessage
    {
        public string TenantName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
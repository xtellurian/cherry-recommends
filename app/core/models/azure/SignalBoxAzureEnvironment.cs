namespace SignalBox.Core
{
    public class SignalBoxAzureEnvironment : SqlServerCredentials
    {
        public string SubscriptionId { get; set; }
        public string SqlServerAzureId { get; set; }
        public string ElasticPoolName { get; set; }
    }
}
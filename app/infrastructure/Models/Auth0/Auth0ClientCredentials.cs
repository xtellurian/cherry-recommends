namespace SignalBox.Infrastructure
{
    public abstract class Auth0ClientCredentials
    {
        public string DefaultAudience { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Endpoint { get; set; }
        public string Domain { get; set; }
    }
}
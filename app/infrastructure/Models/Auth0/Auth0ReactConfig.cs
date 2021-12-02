namespace SignalBox.Infrastructure
{
    public class Auth0ReactConfig : Auth0ClientCredentials
    {
        public string ManagementAudience { get; set; }
        public string Scope { get; set; }
    }
}
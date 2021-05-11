namespace SignalBox.Web.Config
{
    public class Auth0ReactConfig
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string Audience { get; set; }
        public string ManagementAudience { get; set; }
        public string Scope { get; set; }
    }
}
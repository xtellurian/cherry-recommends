namespace SignalBox.Client
{
    public class Connection
    {
        public string Host { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public void Verify()
        {
            if (string.IsNullOrEmpty(Host))
            {
                throw new SignalBoxException("Host is required");
            }
            if (string.IsNullOrEmpty(ClientId))
            {
                throw new SignalBoxException("ClientId is required");
            }
            if (string.IsNullOrEmpty(ClientSecret))
            {
                throw new SignalBoxException("ClientSecret is required");
            }
        }
    }
}
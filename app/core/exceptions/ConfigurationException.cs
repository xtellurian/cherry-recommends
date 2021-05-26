namespace SignalBox.Core
{
    /// <summary>
    /// Thrown when a user tries to misconfigure a resource.
    /// </summary>
    public class ConfigurationException : SignalBoxException
    {
        public ConfigurationException(string message) : base(message)
        {
        }
    }
}
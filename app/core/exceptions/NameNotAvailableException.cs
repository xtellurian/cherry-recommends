namespace SignalBox.Core
{
    public class NameNotAvailableException : SignalBoxException
    {
        public NameNotAvailableException(string name) : base($"The name '{name}' is not available")
        { }
    }
}
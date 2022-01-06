namespace SignalBox.Core
{
    public class DependencyException : SignalBoxException
    {
        public DependencyException(string message) : base("An external dependency failed", message)
        { }
    }
}
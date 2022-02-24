namespace SignalBox.Core
{
    public class DependencyException : SignalBoxException
    {
        public DependencyException(string message) : base("An external dependency failed", message)
        { }
        public DependencyException(string message, System.Exception inner) : base("An external dependency failed", message, inner)
        { }
    }
}
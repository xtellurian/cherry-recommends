namespace SignalBox.Core
{
    /// <summary>
    /// Thrown when a request arguments are invalid.
    /// </summary>
    public class BadRequestException : SignalBoxException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
namespace SignalBox.Core
{
    /// <summary>
    /// Thrown when a request arguments are invalid.
    /// </summary>
    public class BadRequestException : SignalBoxException
    {
        public BadRequestException(string title, System.Exception inner) : base(title, inner)
        { }

        public BadRequestException(string message) : base("Bad Request", message)
        {
            this._status = 400;
        }
        public BadRequestException(string title, string message) : base(title, message)
        {
            this._status = 400;
        }
    }
}
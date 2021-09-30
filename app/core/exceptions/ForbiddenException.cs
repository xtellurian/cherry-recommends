namespace SignalBox.Core
{
    public class ForbiddenException : SignalBoxException
    {
        public ForbiddenException(string title, string message) : base(title, message)
        { }

        public override int Status => 403;
    }
}
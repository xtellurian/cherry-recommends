namespace SignalBox.Core
{
    public class IntegratedSystemException : SignalBoxException
    {
        public IntegratedSystemException(string title, System.Exception ex) : base(title, ex)
        {
        }
        public IntegratedSystemException(string title, string message, System.Exception ex) : base(title, message, ex)
        {
        }
    }
}
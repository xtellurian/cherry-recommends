namespace SignalBox.Core
{
    public class IntegratedSystemException : SignalBoxException
    {
        public IntegratedSystemException(string title, System.Exception ex) : base(title, ex)
        {
        }
    }
}
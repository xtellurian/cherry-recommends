namespace SignalBox.Core
{
    public class WorkflowCancelledException : SignalBoxException
    {
        public WorkflowCancelledException(string message) : base(message)
        {
        }
    }
}
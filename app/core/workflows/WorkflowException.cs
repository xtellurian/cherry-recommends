using System;
using SignalBox.Core.Workflows;

namespace SignalBox.Core
{
    public class WorkflowException : SignalBoxException
    {
        public WorkflowException(string message) : base(message)
        {
        }

        public WorkflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
using System;

namespace SignalBox.Core
{
    public class ScheduledJobException : SignalBoxException
    {
        public ScheduledJobException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
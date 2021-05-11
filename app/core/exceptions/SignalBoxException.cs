using System;

namespace SignalBox.Core
{
    public abstract class SignalBoxException : System.Exception
    {
        protected SignalBoxException(string message) : base(message)
        {
        }

        protected SignalBoxException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
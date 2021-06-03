using System;

namespace SignalBox.Core
{
#nullable enable
    public abstract class SignalBoxException : System.Exception
    {
        private string? _title;

        protected SignalBoxException(string message) : base(message)
        {
        }
        protected SignalBoxException(string title, string message) : base(message)
        {
            this._title = title;
        }

        protected SignalBoxException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected SignalBoxException(string title, string message, Exception innerException) : base(message, innerException)
        {
            this._title = title;
        }

        public string Title => this._title ?? "Internal Error";
    }
}
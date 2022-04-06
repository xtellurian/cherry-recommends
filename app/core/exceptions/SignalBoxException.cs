using System;

namespace SignalBox.Core
{
#nullable enable
    public abstract class SignalBoxException : System.Exception
    {
        private string? _title;
        protected int? _status;

        protected SignalBoxException(string title) : base(title)
        {
            this._title = title;
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

        public virtual string Title => this._title ?? "Internal Error";
        public virtual int Status => this._status ?? 500;
    }
}
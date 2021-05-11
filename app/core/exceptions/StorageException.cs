using System;

namespace SignalBox.Core
{
    public class StorageException : SignalBoxException
    {
        public StorageException(string message) : base(message)
        {
        }

        public StorageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
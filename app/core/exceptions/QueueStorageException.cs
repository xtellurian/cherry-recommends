using System;

namespace SignalBox.Core
{
    public class QueueStorageException : StorageException
    {
        public QueueStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
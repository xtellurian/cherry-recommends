
namespace SignalBox.Core
{
    public class InvalidStorageAccessException : StorageException
    {
        public InvalidStorageAccessException(string message) : base(message)
        {
        }
    }
}
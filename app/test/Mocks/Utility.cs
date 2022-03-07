using Microsoft.Extensions.Logging;
using Moq;
using SignalBox.Core;

namespace SignalBox.Test
{
    public static class Utility
    {
        public static Mock<ILogger<T>> MockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
        public static Mock<IStorageContext> MockStorageContext()
        {
            return new Mock<IStorageContext>();
        }
    }
}
using Microsoft.Extensions.Logging;
using Moq;

namespace SignalBox.Test
{
    public static class Utility
    {
        public static Mock<ILogger<T>> MockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
    }
}
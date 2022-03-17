using System;
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
        public static Mock<ITelemetry> MockTelemetry()
        {
            return new Mock<ITelemetry>();
        }
        public static Mock<IStorageContext> MockStorageContext()
        {
            return new Mock<IStorageContext>();
        }
        public static TestDateTimeProvider DateTimeProvider(DateTimeOffset? value = null)
        {
            return new TestDateTimeProvider(value ?? DateTimeOffset.Now);
        }
    }
}
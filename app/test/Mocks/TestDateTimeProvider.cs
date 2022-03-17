using System;
using SignalBox.Core;

namespace SignalBox.Test
{
    public class TestDateTimeProvider : IDateTimeProvider
    {
        public TestDateTimeProvider(DateTimeOffset value)
        {
            Value = value;
        }

        public DateTimeOffset? Value { get; set; }
        public DateTimeOffset Now => Value ?? DateTime.Now;
    }
}
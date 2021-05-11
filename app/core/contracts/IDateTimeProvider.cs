using System;

namespace SignalBox.Core
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
    }
}
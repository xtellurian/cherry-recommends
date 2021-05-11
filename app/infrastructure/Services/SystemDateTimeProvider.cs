using System;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Services
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now => DateTime.Now;
    }
}
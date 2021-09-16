using System;

namespace SignalBox.Core
{
    public static class EventKindExtensions
    {
#nullable enable
        public static EventKinds ToEventKind(this string? kind)
        {
            if (string.IsNullOrEmpty(kind))
            {
                return EventKinds.Custom;
            }

            if (Enum.TryParse<EventKinds>(kind, out var k))
            {
                return k;
            }
            else
            {
                return EventKinds.Custom;
            }
        }
    }
}
using System;

namespace SignalBox.Core
{
    public interface IBackgroundJob
    {
        DateTimeOffset? LastEnqueued { get; set; }
        DateTimeOffset? LastCompleted { get; set; }
    }
}
using System.Collections.Generic;

namespace SignalBox.Core
{
#nullable enable
    public interface IModelInput
    {
        string? CommonUserId { get; set; }
        IDictionary<string, object>? Arguments { get; set; }
        IDictionary<string, object>? Features { get; set; }
    }
}
using System.Collections.Generic;

namespace SignalBox.Core
{
    public interface IModelInput
    {
        string CommonUserId { get; set; }
        Dictionary<string, object> Arguments { get; set; }
    }
}
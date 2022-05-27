using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
#nullable enable
    public interface IModelInput
    {
        string? GetCustomerId();
        string? CommonUserId { get; }
        string? CustomerId { get; set; }
        string? BusinessId { get; set; }
        IDictionary<string, object>? Arguments { get; set; }
        IDictionary<string, object>? Features { get; set; }
        IDictionary<string, object>? Metrics { get; set; }
        IEnumerable<ParameterBounds>? ParameterBounds { get; set; }
    }
}
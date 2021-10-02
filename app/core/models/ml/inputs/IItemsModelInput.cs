using System.Collections.Generic;

namespace SignalBox.Core
{
#nullable enable
    public interface IItemsModelInput : IModelInput
    {
        IEnumerable<RecommendableItem>? Items { get; set; }
    }
}
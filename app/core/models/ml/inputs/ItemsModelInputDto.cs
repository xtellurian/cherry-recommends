using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Core
{
    public class ItemsModelInputDto : ModelInputDto, IItemsModelInput
    {
        public ItemsModelInputDto()
        { }
        public ItemsModelInputDto(string customerId) : base(customerId)
        { }
        public ItemsModelInputDto(IDictionary<string, object> arguments) : base(arguments)
        { }

        public IEnumerable<RecommendableItem> Items { get; set; }
    }
}
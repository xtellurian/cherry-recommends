using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Core
{
    public class ItemsModelInputDto : ModelInputDto, IItemsModelInput
    {
        public ItemsModelInputDto()
        { }
        public ItemsModelInputDto(string commonUserId) : base(commonUserId)
        { }
        public ItemsModelInputDto(string commonUserId, IDictionary<string, object> arguments) : base(commonUserId, arguments)
        {
        }

        public IEnumerable<RecommendableItem> Items { get; set; }
    }
}
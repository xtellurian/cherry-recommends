using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class CreateRecommendableItemDto : CommonDtoBase
    {
        [Required]
        public double? ListPrice { get; set; }
        public double? DirectCost { get; set; }
        public string Description { get; set; }
        public DynamicPropertyDictionary Properties { get; set; }
    }
}
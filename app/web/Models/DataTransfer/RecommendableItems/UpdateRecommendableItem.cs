using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class UpdateRecommendableItem : DtoBase
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public double? ListPrice { get; set; }
        public double? DirectCost { get; set; }
        public string? Description { get; set; }
        public DynamicPropertyDictionary? Properties { get; set; }
    }
}
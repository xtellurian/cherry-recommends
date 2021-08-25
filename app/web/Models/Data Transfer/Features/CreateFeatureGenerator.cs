
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateFeatureGenerator : DtoBase
    {
        [Required]
        public string FeatureCommonId { get; set; }
        [RegularExpression("MonthsSinceEarliestEvent", ErrorMessage = "GeneratorType must be one of MonthsSinceEarliestEvent")]
        public string GeneratorType { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateRewardSelectorDto
    {
        public string Category { get; set; }
        [Required]
        public string ActionName { get; set; }
        [Required]
        [RegularExpression("Revenue", ErrorMessage = "SelectorType must be one of Revenue")]
        public string SelectorType { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class FilterSelectAggregateStepDto : DtoBase
    {
        [Required]
        [RegularExpression("Filter|Select|Aggregate",
            ErrorMessage = "Type must be one of Filter, Select, Aggregate")]
        public string Type { get; set; }
        [Required]
        public int Order { get; set; }

        public string EventTypeMatch { get; set; }
        public string PropertyNameMatch { get; set; }
        [RegularExpression("Sum|Mean", ErrorMessage = "Type must be one of Sum, Mean")]
        public string AggregationType { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (this.Type == "Agregate")
            {
                if (string.IsNullOrEmpty(AggregationType))
                {
                    throw new BadRequestException("AggregationType is required for Aggregation steps");
                }
            }
        }
    }
}
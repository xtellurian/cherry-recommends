using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
#nullable enable
    public abstract class CreateRecommenderDtoBase : CommonDtoBase
    {
        [Required]
        public override string? Name { get; set; }
        public long? CloneFromId { get; set; }
        public bool? ThrowOnBadInput { get; set; }
        public bool? RequireConsumptionEvent { get; set; }
        public IEnumerable<CreateOrUpdateRecommenderArgument>? Arguments { get; set; }
    }
}
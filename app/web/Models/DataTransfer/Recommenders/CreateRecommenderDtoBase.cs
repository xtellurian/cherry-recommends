using System;
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
        [Obsolete]
        public bool? ThrowOnBadInput { get; set; }
        [Obsolete]
        public bool? RequireConsumptionEvent { get; set; }
        public RecommenderSettingsDto? Settings { get; set; }
        public IEnumerable<CreateOrUpdateRecommenderArgument>? Arguments { get; set; }
    }
}
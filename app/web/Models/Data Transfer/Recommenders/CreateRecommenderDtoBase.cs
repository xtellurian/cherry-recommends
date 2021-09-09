using System.Collections.Generic;
namespace SignalBox.Web.Dto
{
#nullable enable
    public abstract class CreateRecommenderDtoBase : CommonDtoBase
    {
        public long? CloneFromId { get; set; }
        public bool? ThrowOnBadInput { get; set; }
        public IEnumerable<CreateOrUpdateRecommenderArgument>? Arguments { get; set; }
    }
}
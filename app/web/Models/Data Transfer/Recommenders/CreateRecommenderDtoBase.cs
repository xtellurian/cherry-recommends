namespace SignalBox.Web.Dto
{
    public abstract class CreateRecommenderDtoBase : CommonDtoBase
    {
        public long? CloneFromId { get; set; }
        public bool? ThrowOnBadInput { get; set; }
    }
}
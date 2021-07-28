namespace SignalBox.Web.Dto
{
    public abstract class CreateRecommenderDtoBase : CommonDtoBase
    {
        public bool? ThrowOnBadInput { get; set; }
    }
}
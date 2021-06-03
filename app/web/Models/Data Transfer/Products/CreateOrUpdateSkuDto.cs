namespace SignalBox.Web.Dto
{
    public class CreateOrUpdateSkuDto : DtoBase
    {
        public string Name { get; }
        public string SkuId { get; }
        public string Description { get; }
        public double Price { get; }
    }
}
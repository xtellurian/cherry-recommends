namespace SignalBox.Web.Dto
{
    public class UpdateWebChannelPropertiesDto : DtoBase
    {
#nullable enable
        public string? Host { get; set; }
        public bool? PopupAskForEmail { get; set; }
        public int? PopupDelay { get; set; }
        public long? RecommenderIdToInvoke { get; set; }
    }
}
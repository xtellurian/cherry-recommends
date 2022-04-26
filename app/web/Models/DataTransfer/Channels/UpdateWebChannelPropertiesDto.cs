namespace SignalBox.Web.Dto
{
    public class UpdateWebChannelPropertiesDto : DtoBase
    {
#nullable enable
        public string? Host { get; set; }
        public bool? PopupAskForEmail { get; set; }
        public int? PopupDelay { get; set; }
        public long? RecommenderId { get; set; }
        public string? PopupHeader { get; set; }
        public string? PopupSubheader { get; set; }
        public string? CustomerIdPrefix { get; set; }
    }
}
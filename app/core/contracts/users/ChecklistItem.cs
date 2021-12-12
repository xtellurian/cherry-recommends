namespace SignalBox.Core.Internal
{
#nullable enable
    public class CheckistItem
    {
        public bool? complete { get; set; }
        public bool? current { get; set; }
        public bool? next { get; set; }
        public int? order { get; set; }
        public string? label { get; set; }
        public string? description { get; set; }
        public string? actionTo { get; set; }
        public string? actionLabel { get; set; }
        public string? docsLink { get; set; }
    }
}
namespace SignalBox.Core
{
#nullable enable
    public class NextPageInfo
    {
        public NextPageInfo(string? after)
        {
            After = after;
        }

        public string? After { get; set; }
    }
}
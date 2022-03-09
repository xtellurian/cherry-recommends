using SignalBox.Infrastructure;

namespace SignalBox.Web
{
#nullable enable
    public class ReactConfig
    {
        public SegmentConfig? Segment { get; set; }
        public HotjarConfig? Hotjar { get; set; }
        public LaunchDarklyConfig? LaunchDarkly { get; set; }
        public Auth0ReactConfig? Auth0 { get; set; }
    }
}
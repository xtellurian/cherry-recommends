using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
    public class IframeAction : CrmCardAction
    {
        public override string Type { get; protected set; } = "IFRAME";
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
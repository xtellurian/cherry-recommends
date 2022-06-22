using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class PopupCondition
    {
        public string Id { get; set; }
        public string Parameter { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
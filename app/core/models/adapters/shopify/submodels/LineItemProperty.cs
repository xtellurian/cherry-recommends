using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    /// <summary>
    /// An object representing a properties for <see cref="LineItem.Properties"/>
    /// </summary>
    public class LineItemProperty
    {
        /// <summary>
        /// The name of the note attribute.
        /// </summary>
        [JsonPropertyName("name")]
        public object Name { get; set; }

        /// <summary>
        /// The value of the note attribute.
        /// </summary>
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}
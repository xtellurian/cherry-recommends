using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    /// <summary>
    /// An object representing a note attribute for <see cref="Order.NoteAttributes"/>
    /// </summary>
    public class NoteAttribute
    {
        /// <summary>
        /// The name of the note attribute.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The value of the note attribute.
        /// </summary>
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}
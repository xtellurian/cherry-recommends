using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Touchpoint : CommonEntity
    {
        public Touchpoint()
        { }
        public Touchpoint(string commonId, string name) : base(commonId, name)
        { }

        [JsonIgnore]
        public ICollection<TrackedUserTouchpoint> TrackedUserTouchpoints { get; set; }
    }
}
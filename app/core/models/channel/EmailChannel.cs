using System.Collections.Generic;

namespace SignalBox.Core
{
    public class EmailChannel : ChannelBase
    {
        protected EmailChannel()
        { }

        public EmailChannel(string name, IntegratedSystem linkedSystem)
            : base(name, ChannelTypes.Email, linkedSystem)
        {
        }

        /// <summary>
        /// ID of the list that triggers the Email flow
        /// </summary>
        public string ListTriggerId { get; set; }

        /// <summary>
        /// Name of the list that triggers the Email flow
        /// </summary>
        public string ListTriggerName { get; set; }

        public override IDictionary<string, object> Properties =>
        new Dictionary<string, object>
        {
            {"listTriggerId", ListTriggerId},
            {"listTriggerName", ListTriggerName}
        };
    }
}

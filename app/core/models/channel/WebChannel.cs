using System.Collections.Generic;
using SignalBox.Core.Integrations.Website;

namespace SignalBox.Core
{

    public class WebChannel : ChannelBase
    {
        protected WebChannel()
        { }

        public WebChannel(string name, IntegratedSystem linkedSystem)
            : base(name, ChannelTypes.Web, linkedSystem)
        {
        }

        /// <summary>
        /// Website host / url
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Flag indicating whether a popup asking for email will be displayed or not
        /// </summary>
        public bool? PopupAskForEmail { get; set; }
        /// <summary>
        /// Delay in displaying the popup after page load, in milliseconds
        /// </summary>
        public int? PopupDelay { get; set; }
        /// <summary>
        /// Recommender Id to invoke for popup offering promotions
        /// </summary>
        public long? RecommenderIdToInvoke { get; set; }
        /// <summary>
        /// Popup header
        /// </summary>
        public string PopupHeader { get; set; }
        /// <summary>
        /// Popup subheader
        /// </summary>
        public string PopupSubheader { get; set; }
        /// <summary>
        /// Prefix added to the random customer id
        /// </summary>
        public string CustomerIdPrefix { get; set; }
        public override IDictionary<string, object> Properties =>
        new Dictionary<string, object>
        {
            {"host", Host},
            {"popupAskForEmail", PopupAskForEmail},
            {"popupDelay", PopupDelay},
            {"recommenderIdToInvoke", RecommenderIdToInvoke},
            {"popupHeader", PopupHeader},
            {"popupSubheader", PopupSubheader},
            {"customerIdPrefix", CustomerIdPrefix}
        };

#nullable enable
        public string? ApplicationSecret => (this.LinkedIntegratedSystem as WebsiteIntegratedSystem)?.ApplicationSecret;
    }
}

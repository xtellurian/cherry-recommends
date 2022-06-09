using System.Collections.Generic;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    /// <summary> Average Purchase Value report </summary>
    public class APVReportDto : DtoBase
    {
        /// <summary> Campaign id. </summary>
        public long CampaignId { get; set; }
        /// <summary> Report type. </summary>
        public DateTimePeriod Type { get; set; }
        /// <summary> Data for the report. </summary>
        public IEnumerable<OfferMeanGrossRevenue> Data { get; set; }
    }
}
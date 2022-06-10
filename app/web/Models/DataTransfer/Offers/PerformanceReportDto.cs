using System.Collections.Generic;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class PerformanceReportDto : DtoBase
    {
        /// <summary> Campaign id. </summary>
        public long CampaignId { get; set; }
        /// <summary> ARPO report type. </summary>
        public DateTimePeriod Type { get; set; }
        /// <summary> Data for the ARPO report. </summary>
        public IEnumerable<OfferMeanGrossRevenue> Data { get; set; }
    }
}
using System.Collections.Generic;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class OfferConversionRateReportDto : DtoBase
    {
        /// <summary> Campaign id. </summary>
        public long CampaignId { get; set; }
        /// <summary> Report period type. </summary>
        public DateTimePeriod Type { get; set; }
        /// <summary> Data for the report. </summary>
        public IEnumerable<OfferConversionRateData> Data { get; set; }
    }
}
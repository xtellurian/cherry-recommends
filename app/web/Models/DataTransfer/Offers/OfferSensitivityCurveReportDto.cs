using System;
using System.Collections.Generic;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class OfferSensitivityCurveReportDto : DtoBase
    {
        /// <summary> Campaign id. </summary>
        public long CampaignId { get; set; }
        /// <summary> Start date of the report data. </summary>
        public DateTimeOffset DataSinceDate { get; set; }
        /// <summary> Data for the report. </summary>
        public IEnumerable<OfferSensitivityCurveData> Data { get; set; }
    }
}
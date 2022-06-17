using System;

namespace SignalBox.Core
{
    public class OfferSensitivityCurveData
    {
        public OfferSensitivityCurveData()
        { }

        public long PromotionId { get; set; }
        public string PromotionName { get; set; }
        public double TotalGrossRevenue { get; set; }
        public double MeanGrossRevenue { get; set; }
        public int TotalCount { get; set; }
        public int RedeemedCount { get; set; }
        public double ConversionRate { get; set; }
    }
}


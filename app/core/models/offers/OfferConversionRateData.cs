using System;

namespace SignalBox.Core
{
    public class OfferConversionRateData
    {
        public OfferConversionRateData()
        { }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RedeemedCount { get; set; }
        public int NonBaselineRedeemedCount { get; set; }
        public int BaselineRedeemedCount { get; set; }
        public int TotalCount { get; set; }
        public int NonBaselineCount { get; set; }
        public int BaselineCount { get; set; }
        public double ConversionRate { get; set; }
        public double NonBaselineConversionRate { get; set; }
        public double BaselineConversionRate { get; set; }
    }
}
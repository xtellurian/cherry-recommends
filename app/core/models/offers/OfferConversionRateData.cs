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
        public int TotalCount { get; set; }
        public double ConversionRate { get; set; }
    }
}
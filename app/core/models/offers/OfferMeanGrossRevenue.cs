using System;

namespace SignalBox.Core
{
    public class OfferMeanGrossRevenue
    {
        public OfferMeanGrossRevenue()
        { }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalGrossRevenue { get; set; }
        public double MeanGrossRevenue { get; set; }
        public double BaselineMeanGrossRevenue { get; set; }
        public int DistinctCustomerCount { get; set; }
        public int OfferCount { get; set; }
    }
}
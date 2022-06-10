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
        public double NonBaselineTotalGrossRevenue { get; set; }
        public double BaselineTotalGrossRevenue { get; set; }
        public double MeanGrossRevenue { get; set; }
        public double NonBaselineMeanGrossRevenue { get; set; }
        public double BaselineMeanGrossRevenue { get; set; }
        public int DistinctCustomerCount { get; set; }
        public int OfferCount { get; set; }
        public int NonBaselineOfferCount { get; set; }
        public int BaselineOfferCount { get; set; }

        // safe possible divide by zero - whole things goes to null then zero
        public double AdditionalRevenue => (NonBaselineMeanGrossRevenue - BaselineMeanGrossRevenue) * NonBaselineOfferCount;
    }
}
using System;

namespace SignalBox.Core
{
    public class MetricDailyBinValueNumeric
    {
        public MetricDailyBinValueNumeric()
        { }
        public double BinFloor { get; set; }
        public double BinWidth { get; set; }
        public string BinRange { get; set; }
        public int CustomerCount { get; set; }
        public int BusinessCount { get; set; }
    }
}
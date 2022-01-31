using SignalBox.Core;
using System.Collections.Generic;

namespace SignalBox.Functions
{
    public class ScoredItem
    {
        public ScoredItem(string commonId, double score)
        {
            CommonId = commonId;
            Score = score;
        }
        
        public string CommonId { get; set; }
        public double Score { get; set; }
    }
    public class InvokeCategoricalOptimiserResponse
    {
        public List<ScoredItem> ScoredItems { get; set; }
    }

}
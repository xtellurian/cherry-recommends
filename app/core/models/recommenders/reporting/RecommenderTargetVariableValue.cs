using System;

namespace SignalBox.Core.Campaigns
{
    public class RecommenderTargetVariableValue : Entity
    {
        protected RecommenderTargetVariableValue() { }
        public RecommenderTargetVariableValue(long recommenderId,
                                              int version,
                                              DateTimeOffset start,
                                              DateTimeOffset end,
                                              string name,
                                              double value)
        {
            this.RecommenderId = recommenderId;
            this.Version = version;
            this.Start = start;
            this.End = end;
            this.Name = name;
            this.Value = value;
        }

        public long RecommenderId { get; set; }
        public int Version { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
namespace SignalBox.Core.Recommenders
{
#nullable enable
    public class RecommenderErrorHandling : RecommenderSettings
    {
        public RecommenderErrorHandling() { }
        public RecommenderErrorHandling(RecommenderSettings? other)
        {
            this.ThrowOnBadInput = other?.ThrowOnBadInput;
            this.RequireConsumptionEvent = other?.RequireConsumptionEvent;
        }
    }
}